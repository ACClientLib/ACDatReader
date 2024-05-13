﻿using ACDatReader.IO;
using ACDatReader.Tests.Lib;
using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACDatReader.Tests.IO {
    [TestClass]
    public class DatFileReaderTests {
        [TestMethod]
        public void CanReadMultipleValues() {
            var bytes = new byte[12];
            var bSpan = new Span<byte>(bytes);
            BinaryPrimitives.WriteUInt32LittleEndian(bSpan.Slice(0), 1);
            BinaryPrimitives.WriteInt32LittleEndian(bSpan.Slice(4), -1);
            BinaryPrimitives.WriteUInt32LittleEndian(bSpan.Slice(8), 0);

            var reader = new DatFileReader(bytes);

            Assert.AreEqual(1u, reader.ReadUInt32());
            Assert.AreEqual(-1, reader.ReadInt32());
            Assert.AreEqual(0u, reader.ReadUInt32());
        }

        [TestMethod]
        public void CanSkipAndRead() {
            var bytes = new byte[12];
            var bSpan = new Span<byte>(bytes);
            BinaryPrimitives.WriteUInt32LittleEndian(bSpan.Slice(0), 1);
            BinaryPrimitives.WriteInt32LittleEndian(bSpan.Slice(4), -1);
            BinaryPrimitives.WriteUInt32LittleEndian(bSpan.Slice(8), 0);

            var reader = new DatFileReader(bytes);
            reader.Skip(4);
            Assert.AreEqual(-1, reader.ReadInt32());
            Assert.AreEqual(0u, reader.ReadUInt32());
        }

        [TestMethod]
        [CombinatorialData]
        public void CanReadUInt32([DataValues(1234u, 5678u, 0u, 1u, 0xFFFFFFFFu)] uint number) {
            var bytes = new byte[4];
            BinaryPrimitives.WriteUInt32LittleEndian(bytes, number);

            var reader = new DatFileReader(bytes);

            Assert.AreEqual(number, reader.ReadUInt32());
        }

        [TestMethod]
        [CombinatorialData]
        public void CanReadInt32([DataValues(-1234, 5678, 0, 1, unchecked((int)0xFFFFFFFF))] int number) {
            var bytes = new byte[4];
            BinaryPrimitives.WriteInt32LittleEndian(bytes, number);

            var reader = new DatFileReader(bytes);

            Assert.AreEqual(number, reader.ReadInt32());
        }

        [TestMethod]
        public void CanReadBytes() {
            var bytes = new byte[100];
            Random.Shared.NextBytes(bytes);

            var reader = new DatFileReader(bytes);
            var readBytes = reader.ReadBytes(bytes.Length);

            CollectionAssert.AreEqual(bytes, readBytes);
        }
    }
}