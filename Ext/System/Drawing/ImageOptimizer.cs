using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.Diagnostics;

using Ext.System.Core.Progress;
using Ext.System.Drawing;

namespace Ext.System.Drawing {
    public class ImageOptimizer {

        private class IndexedColorsComparerPixelsCount : IComparer<IndexedColor> {
            public int Compare(IndexedColor x, IndexedColor y) {
                return x.PixelsCount.CompareTo(y.PixelsCount);
            }
        }

        private class IndexedColor : IComparable<IndexedColor> {

            public static readonly IComparer<IndexedColor> AlternativeComparer = new IndexedColorsComparerPixelsCount();

            public Color Color;
            public int Index;
            public List<Pixel> Pixels;
            public List<IndexedColor> Replaced;
            public int PixelsCount { get; protected set; }
            public IndexedColor(Color color) {
                this.Color = color;
                Pixels = new List<Pixel>();
                Replaced = new List<IndexedColor>();
            }

            public int CompareTo(IndexedColor other) {
                int a = GetHashCode();
                int b = other.GetHashCode();
                return a.CompareTo(b);
            }

            public override int GetHashCode() {
                return Color.R << 16 | Color.G << 8 | Color.B;
            }

            public void ReplaceColor(Bitmap image, Color color) {
                foreach(var v in Pixels)
                    image.SetPixel(v.X, v.Y, color);
            }

            public void ResetChanges(Bitmap image) {
                foreach(var v in Pixels)
                    image.SetPixel(v.X, v.Y, v.Color);
            }

            public void ReplaceColorRoot(Bitmap image, Color color) {
                ReplaceColor(image, color);
                Stack<int> ids = new Stack<int>();
                Stack<IndexedColor> path = new Stack<IndexedColor>();
                var IndexedColor = this;
                int id = 0;
                while(true) {
                    if(IndexedColor.Replaced.Count > id) {
                        path.Push(IndexedColor);
                        ids.Push(id);
                        IndexedColor = IndexedColor.Replaced[id];
                        IndexedColor.ReplaceColor(image, color);
                    } else {
                        if(ids.Count == 0)
                            break;
                        id = ids.Pop();
                        IndexedColor = path.Pop();
                        id++;
                    }
                }
            }

            public void ResetChangesRoot(Bitmap image) {
                ResetChanges(image);
                Stack<int> ids = new Stack<int>();
                Stack<IndexedColor> path = new Stack<IndexedColor>();
                var IndexedColor = this;
                int id = 0;
                while(true) {
                    if(IndexedColor.Replaced.Count > id) {
                        path.Push(IndexedColor);
                        ids.Push(id);
                        IndexedColor = IndexedColor.Replaced[id];
                        IndexedColor.ResetChanges(image);
                    } else {
                        if(ids.Count == 0)
                            break;
                        id = ids.Pop();
                        IndexedColor = path.Pop();
                        id++;
                    }
                }
            }

            public void UpdatePixelsCount() {
                //Stack<int> ids = new Stack<int>();
                //Stack<IndexedColor> path = new Stack<IndexedColor>();
                //var IndexedColor = this;
                //int id = 0;
                //PixelsCount = 0;
                //while(true) {
                //    if(IndexedColor.Replaced.Count > id) {
                //        path.Push(IndexedColor);
                //        ids.Push(id);
                //        IndexedColor = IndexedColor.Replaced[id];
                //        PixelsCount += IndexedColor.Pixels.Count;
                //    } else {
                //        if(ids.Count == 0)
                //            break;
                //        id = ids.Pop();
                //        IndexedColor = path.Pop();
                //        id++;
                //    }
                //}
                PixelsCount = Pixels.Count;
            }

            public void AddReplacment(IndexedColor color) {
                if(Replaced.Contains(color))
                    return;
                Replaced.Add(color);
                PixelsCount += color.PixelsCount;
            }

            public void RemoveReplacment(IndexedColor color) {
                if(Replaced.Contains(color)) {
                    Replaced.Remove(color);
                    PixelsCount -= color.PixelsCount;
                } else
                    throw new IndexOutOfRangeException();
            }

        }

        private Bitmap _image;
        private DefaultProgress _progress;
        private IndexedColor[] _colorsSorted;
        private HashSet<int> _deletedColors = new HashSet<int>();
        private Stack<int> _deletedColorReplacments = new Stack<int>();
        private IndexedColor[] _colorsBook;

        public IProgressExt Progress;
        public int ColorsCount
        {
            get
            {
                if(_colorsSorted == null || _deletedColors == null)
                    return 0;
                return _colorsSorted.Length - _deletedColors.Count;
            }
        }

        public ImageOptimizer(Bitmap image, bool updateColors) {
            _progress = new DefaultProgress();
            Progress = _progress;
            _image = image;
            if(updateColors)
                UpdateColors();
        }

        public void UpdateColors() {
            _progress.OperationsDone = 0;
            _progress.OperationsTotal = _image.Width;
            Dictionary<Color, IndexedColor> colors = new Dictionary<Color, IndexedColor>();
            for(int i = 0; i < _image.Width; i++) {
                for(int j = 0; j < _image.Height; j++) {
                    var col = _image.GetPixel(i, j);
                    if(colors.ContainsKey(col)) {
                        colors[col].Pixels.Add(new Pixel(col, i, j));
                    } else {
                        var indexedColor = new IndexedColor(col);
                        indexedColor.Pixels.Add(new Pixel(col, i, j));
                        colors[col] = indexedColor;
                    }
                }
                _progress.OperationsDone = i;
            }
            _colorsSorted = colors.Values.ToArray();
            _colorsBook = new IndexedColor[_colorsSorted.Length];
            Array.Copy(_colorsSorted, _colorsBook, _colorsSorted.Length);
            Array.Sort(_colorsBook);
            for(int i = 0; i < _colorsBook.Length; i++)
                _colorsBook[i].Index = i;
            for(int i = 0; i < _colorsSorted.Length; i++)
                _colorsSorted[i].UpdatePixelsCount();
            Array.Sort(_colorsSorted, (IndexedColor a, IndexedColor b) => a.PixelsCount.CompareTo(b.PixelsCount));
            _progress.OperationsDone = _progress.OperationsTotal;
        }

        public Task UpdateColorsAsync() {
            return Task.Factory.StartNew(UpdateColors);
        }

        public Task DeleteColorsAsync(int count) {
            return Task.Factory.StartNew(() => DeleteColors(count));
        }

        public Task RestoreColorsAsync(int count) {
            return Task.Factory.StartNew(() => RestoreColors(count));
        }

        private int GetClosestColor(IndexedColor cl) {
            //Color minColor = _colorsSorted[_colorsSorted.Length - 1].Key.color;
            //var min = ColorAndImageFactory.GetColorRangeQ(cl, minColor);
            //for(int i = _colorsSorted.Length - 2; i > _deletedColors.Count; i--) {
            //    var diff = ColorAndImageFactory.GetColorRangeQ(cl, _colorsSorted[i].Key);
            //    if(diff < min) {
            //        minColor = _colorsSorted[i].Key;
            //        min = diff;
            //    }
            //}
            //return minColor;
            //throw new NotImplementedException();
            int id = cl.Index;
            int diff = 0;
            int iteration = 1;
            do {
                if(diff > 0) {
                    diff -= iteration;
                    if(id + diff < 0)
                        continue;
                } else {
                    diff += iteration;
                    if(id + diff > _colorsBook.Length - 1)
                        continue;
                }

                iteration++;
            } while(_deletedColors.Contains(id + diff));
            //Trace.WriteLine(string.Format("{0} -> {1}, {2} -> {3}", id, id + diff, _colorsBook[id].Index, _colorsBook[id + diff].Index));
            return id + diff;
        }

        private void FlushRemoval(int index, Color color) {
            foreach(var v in _colorsSorted[index].Pixels) {
                _image.SetPixel(v.X, v.Y, color);
            }
        }

        private void UndoRemoval(int id) {
            foreach(var v in _colorsSorted[id].Pixels) {
                _image.SetPixel(v.X, v.Y, v.Color);
            }
        }

        private void DeleteColor(int index) {
            IndexedColor cl = _colorsSorted[index];
            int replacment = GetClosestColor(cl);
            cl.ReplaceColorRoot(_image, _colorsBook[replacment].Color);
            _colorsBook[replacment].AddReplacment(cl);
            //_colorsBook[replacment].UpdatePixelsCount();
            //FlushRemoval(index, _colorsBook[replacment].Color);
            _deletedColors.Add(replacment);
            _deletedColorReplacments.Push(replacment);
            UpdatePositionIncrease(replacment);
            //for(int i = 0; i < _colorsSorted.Length; i++)
            //Array.Sort(_colorsSorted, _deletedColors.Count, _colorsSorted.Length - _deletedColors.Count, IndexedColor.AlternativeComparer);
        }

        private void UpdatePositionIncrease(int index) {
            IndexedColor swap = null;
            while(index + 1 < _colorsSorted.Length && _colorsSorted[index].PixelsCount > _colorsSorted[index + 1].PixelsCount) {
                int iSwap = _colorsSorted[index].Index;
                _colorsSorted[index].Index = _colorsSorted[index + 1].Index;
                _colorsSorted[index + 1].Index = iSwap;
                swap = _colorsSorted[index];
                _colorsSorted[index] = _colorsSorted[index + 1];
                _colorsSorted[index + 1] = swap;
                index++;
            }
        }

        private void UpdatePositionDecrease(int index, int minimalIndex) {
            IndexedColor swap = null;
            while(index - 1 >= minimalIndex && _colorsSorted[index].PixelsCount < _colorsSorted[index - 1].PixelsCount) {
                swap = _colorsSorted[index];
                _colorsSorted[index] = _colorsSorted[index - 1];
                _colorsSorted[index - 1] = swap;
                index--;
            }
        }

        private void RestoreColor(int index) {
            //UndoRemoval(index);
            var replacment = _deletedColorReplacments.Pop();
            var ic = _colorsBook[replacment];
            ic.RemoveReplacment(_colorsSorted[index]);
            _colorsSorted[index].ResetChangesRoot(_image);
            UpdatePositionDecrease(replacment, _deletedColors.Count);
            _deletedColors.Remove(replacment);
        }

        public void DeleteColors(int count) {
            _progress.OperationsDone = 0;
            _progress.OperationsTotal = count;
            for(int i = 0; i < count; i++) {
                DeleteColor(_deletedColors.Count);
                _progress.OperationsDone++;
            }
        }

        public void RestoreColors(int count) {
            _progress.OperationsDone = 0;
            _progress.OperationsTotal = count;
            for(int i = 0; i < count; i++) {
                RestoreColor(_deletedColors.Count-1);
                _progress.OperationsDone++;
            }
        }

    }
}
