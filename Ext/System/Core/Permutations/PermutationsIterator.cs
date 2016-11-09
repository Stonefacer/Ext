using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ext.System.Core.Permutations {
    public class PermutationsIterator : IEnumerable<string>, IEnumerator<string> {

        private string _source;
        private int[] _levelsIndexes;
        private StringBuilder _current;
        private bool _shortString;
        private bool _reversed;
        private int _startId;
        private bool _firstIteration;

        public PermutationsIterator(string source) {
            var lst = source.ToList();
            lst.Sort();
            _source = string.Join("", lst);
            lst.Clear();
            _current = new StringBuilder(_source);
            if(source.Length > 2) {
                _levelsIndexes = new int[source.Length - 2];
                for(int i = 0; i < source.Length - 2; i++) {
                    _levelsIndexes[i] = i;
                }
                _shortString = false;
                _startId = _levelsIndexes.Length - 1;
            } else {
                _shortString = true;
            }
            _firstIteration = true;
            Current = _current.ToString();
        }

        public string Current { get; private set; }

        object IEnumerator.Current { get { return Current; } }

        public void Dispose() {
            _levelsIndexes?.Clear();
            _current.Clear();
        }

        private bool MoveNextShortString() {
            if(_source.Length < 2) {
                return false;
            }
            if(_reversed) {
                return false;
            }
            var buf = _current[0];
            _current[0] = _current[1];
            _current[1] = buf;
            _reversed = true;
            Current = _current.ToString();
            return true;
        }

        private bool IsAccessable(int levelId, int charId) {
            if(charId < 0 || charId >= _source.Length)
                return false;
            for(int i = 0; i < levelId; i++) {
                if(_levelsIndexes[i] == charId)
                    return false;
            }
            return true;
        }

        private void UpdateString() {
            if(_shortString) {
                return;
            }
            if(_reversed) {
                for(int i = 0; i < _levelsIndexes.Length; i++) {
                    _current[i] = _source[_levelsIndexes[i]];
                }
                int index = 0;
                int offset = -2;
                for(; index < _source.Length && offset != 0; index++) {
                    if(IsAccessable(_levelsIndexes.Length, index)) {
                        _current[_source.Length + offset] = _source[index];
                        offset++;
                    }
                }
            } else {
                var buf = _current[_source.Length - 2];
                _current[_source.Length - 2] = _current[_source.Length - 1];
                _current[_source.Length - 1] = buf;
            }
            Current = _current.ToString();
        }

        private bool UpdateAccessability(int startLevelId) {
            var res = true;
            while(startLevelId < _levelsIndexes.Length) {
                var id = _levelsIndexes[startLevelId];
                while(!IsAccessable(startLevelId, id)) {
                    id--;
                    if(id < 0) {
                        id = _levelsIndexes[startLevelId];
                        while(!IsAccessable(startLevelId, id)) {
                            id++;
                            if(id > _source.Length - 1)
                                id = 0;
                        }
                    }
                    res = false;
                }
                _levelsIndexes[startLevelId] = id;
                startLevelId++;
            }
            return res;
        }

        private bool IncreaseIndex(int levelId) {
            _levelsIndexes[levelId]++;
            var res = true;
            if(_levelsIndexes[levelId] == _source.Length) {
                _levelsIndexes[levelId] = 0;
                res = false;
            }
            while(!IsAccessable(levelId, _levelsIndexes[levelId])) {
                _levelsIndexes[levelId]++;
                if(_levelsIndexes[levelId] == _source.Length) {
                    _levelsIndexes[levelId] = 0;
                    res = false;
                }
            }
            UpdateAccessability(levelId + 1);
            return res;
        }

        public IEnumerator<string> GetEnumerator() {
            return this;
        }

        public bool MoveNext() {
            if(_firstIteration) {
                _firstIteration = false;
                return true;
            }
            if(!_shortString) {
                if(_reversed) {
                    _startId = _levelsIndexes.Length - 1;
                    while(_startId >= 0 && !IncreaseIndex(_startId)) {
                        _startId--;
                    }
                    if(_startId == -1)
                        return false;
                }
                UpdateString();
                _reversed = !_reversed;
                return true;

            } else {
                return MoveNextShortString();
            }
        }

        public void Reset() {
            for(int i = 0; i < _source.Length - 2; i++)
                _levelsIndexes[i] = i;
            _levelsIndexes[_levelsIndexes.Length - 1] = -1;
            _reversed = false;
            _firstIteration = true;
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return this;
        }

    }
}
