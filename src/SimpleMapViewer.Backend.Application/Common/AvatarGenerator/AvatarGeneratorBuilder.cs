using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace SimpleMapViewer.Backend.Application.Common.AvatarGenerator {
    internal class AvatarGeneratorBuilder {
        public static AvatarGeneratorBuilder Build(int size, bool isSymmetry = true) {
            return new AvatarGeneratorBuilder(size, isSymmetry);
        }

        private static readonly IList<Color> Colors = new List<Color> {
            Color.FromArgb(127, 127, 220),
            Color.FromArgb(100, 207, 172),
            Color.FromArgb(198, 87, 181),
            Color.FromArgb(134, 166, 220),
            Color.FromArgb(0xf2, 0x4e, 0x33),
            Color.FromArgb(0xf9, 0x97, 0x40),
            Color.FromArgb(0xf9, 0xc8, 0x2f),
            Color.FromArgb(0x14, 0xad, 0xe3),
            Color.FromArgb(0x9e, 0xd2, 0x00),
            Color.FromArgb(0xb5, 0x4e, 0x33),
            Color.FromArgb(0xb5, 0x44, 0xec),
        };

        private readonly AvatarGenerator _avatarGenerator;

        private AvatarGeneratorBuilder(int size, bool isAvatarSymmetric = true) {
            _avatarGenerator = new AvatarGenerator {
                Size = size,
                Colors = Colors,
                IsAvatarSymmetric = isAvatarSymmetric
            };
        }

        public AvatarGeneratorBuilder SetPadding(int padding) {
            _avatarGenerator.Padding = padding;
            return this;
        }

        public AvatarGeneratorBuilder SetAsymmetry(bool isAvatarSymmetric = true) {
            _avatarGenerator.IsAvatarSymmetric = isAvatarSymmetric;
            return this;
        }

        public AvatarGeneratorBuilder SetBlockSize(int blockSize) {
            _avatarGenerator.BlockSize = blockSize;
            return this;
        }

        public AvatarGeneratorBuilder SetSeed(string seed) {
            if (!string.IsNullOrWhiteSpace(seed)) {
                if (seed.Length < 3)
                    seed = $"RA{seed}";
                _avatarGenerator.Seed = Encoding.UTF8.GetBytes(seed);
            }
            return this;
        }

        public Image ToImage() => _avatarGenerator.Generate();

        public byte[] ToBytes(ImageFormat imageFormat) => ImageToBuffer(ToImage(), imageFormat);

        public static byte[] ImageToBuffer(Image image, ImageFormat imageFormat) {
            if (image == null) return null;

            using var stream = new MemoryStream();
            using var bitmap = new Bitmap(image);

            bitmap.Save(stream, imageFormat);
            stream.Position = 0;
            var data = new byte[stream.Length];
            stream.Read(data, 0, Convert.ToInt32(stream.Length));
            stream.Flush();

            return data;
        }
    }
}