using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace SimpleMapViewer.Backend.Application.Common.AvatarGenerator {
    internal class AvatarGenerator {
        /// <summary>
        /// Size of avatar in pixels
        /// </summary>
        public int Size { get; set; } = 100;

        public int BlockSize { get; set; } = 5;

        public byte[] Seed { get; set; }

        public bool IsAvatarSymmetric { get; set; }
        
        public int Padding { get; set; }

        public IList<Color> Colors { get; set; }
        
        private byte[] _bytes;
        private int _currentByteIndex;
        
        public Image Generate() {
            SetupOptions();
            SetupBytes();
            bool[] blocks = null;
            while (!AreBlocksValid(blocks)) {
                blocks = GenerateBlocks();
            }
          
            return Draw(blocks);
        }

        private Bitmap Draw(bool[] blocks) {
            var canvasSize = Size - Padding * 2;
            var avatar = new Bitmap(canvasSize + Padding * 2, canvasSize + Padding * 2);
            using var graphics = Graphics.FromImage(avatar);

            var size = Size / BlockSize;
            var index = GetNextByte() % Colors.Count;
            var color = Colors[index];

            var whiteBlockSizeX = 0;
            var whiteBlockSizeY = BlockSize / 2;

            graphics.Clear(color);
            for (var y = 0; y < BlockSize; y++) {
                for (var x = 0; x < BlockSize; x++) {
                    if (y < whiteBlockSizeY && x < whiteBlockSizeX) continue;

                    if (!blocks[y * BlockSize + x]) continue;
                    using var brush = new SolidBrush(Color.White);
                    graphics.FillRectangle(
                        brush,
                        new Rectangle(Padding + x * size, Padding + y * size, size, size)
                    );
                }
            }

            return avatar;
        }

        private void SetupOptions() {
            if (Size <= 0) Size = 100;
            if (BlockSize < 3) BlockSize = 5;
        }

        private void SetupBytes() {
            var needUseGivenSeed = Seed != null && Seed.Length > 0;
            _bytes = needUseGivenSeed ? Seed : Guid.NewGuid().ToByteArray();
        }

        private bool AreBlocksValid(bool[] blocks) {
            if (blocks == null) return false;

            var count = blocks.Aggregate(0, (c, b) => b ? c + 1 : c);

            if (BlockSize > 2 && count < 6) return false;
            if (count == BlockSize * BlockSize) return false;

            return true;
        }

        private bool[] GenerateBlocks() {
            var blocks = new bool[BlockSize * BlockSize];
            for (var y = 0; y < BlockSize; y++) {
                for (var x = 0; x < BlockSize; x++) {
                    var index = y * BlockSize + x;
                    if (BlockSize / 2 < x && IsAvatarSymmetric) {
                        blocks[index] = blocks[index - x + BlockSize - x - 1];
                    } else {
                        blocks[index] = (GetNextByte() & 1) == 0;
                    }
                }
            }

            return blocks;
        }

        private byte GetNextByte() {
            if (_currentByteIndex >= _bytes.Length) {
                _currentByteIndex = 0;
                SetupBytes();
            }

            return _bytes[_currentByteIndex++];
        }
    }
}