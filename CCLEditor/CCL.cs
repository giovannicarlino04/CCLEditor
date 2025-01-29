using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCLEditor
{

    public struct Entry
    {
        public Int16 Padding;
        public string shortName;
        public byte nullterm;
        public byte[] Padding1;
        public Int32 Unknown;
        public Int32 Unknown2;
        public byte[] Padding2;
    }
    public class CCL
    {
        public const Int32 Magic = 0x4C434323; //  #CCL
        public const Int32 entrySize = 24;  // 24
        public Int32 slotCount;
        public Entry[] Entries;
        public void Load(string fileName) {
            using (BinaryReader br = new BinaryReader(File.Open(fileName, FileMode.Open)))
            {
                br.BaseStream.Seek(0, SeekOrigin.Begin);
                Int32 magic = br.ReadInt32();
                if (magic != Magic)
                    throw new Exception("Wrong file MAGIC");

                long fileSize = new FileInfo(fileName).Length;
                Int32 offset = 2742;

                slotCount = (int)((fileSize - offset) / entrySize);


                Entries = new Entry[slotCount];

                for(int i = 0; i < slotCount; i++)
                {
                    br.BaseStream.Seek(offset + (entrySize * i), SeekOrigin.Begin);

                    Entries[i].Padding = br.ReadInt16();
                    Entries[i].shortName = Encoding.Default.GetString(br.ReadBytes(3));
                    Entries[i].nullterm = br.ReadByte();
                    Entries[i].Padding1 = br.ReadBytes(8);
                    Entries[i].Unknown = br.ReadInt32();
                    Entries[i].Unknown2 = br.ReadInt32();
                    Entries[i].Padding2 = br.ReadBytes(4);

                }


            }
        }
        public void Save(string fileName)
        {
            if (Entries == null || Entries.Length == 0)
                throw new InvalidOperationException("No entries to save.");

            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Write))
            using (BinaryWriter bw = new BinaryWriter(fs))
            {
                int offset = 2742;

                for (int i = 0; i < slotCount; i++)
                {
                    fs.Seek(offset + (entrySize * i), SeekOrigin.Begin);

                    bw.Write(Entries[i].Padding);

                    byte[] nameBytes = Encoding.Default.GetBytes(Entries[i].shortName.PadRight(3, '\0').Substring(0, 3));
                    bw.Write(nameBytes);

                    bw.Write(Entries[i].nullterm);
                    bw.Write(Entries[i].Padding1);
                    bw.Write(Entries[i].Unknown);
                    bw.Write(Entries[i].Unknown2);
                    bw.Write(Entries[i].Padding2);
                }
            }
        }

        public void AddEntry(Entry newEntry)
        {
            List<Entry> entryList = Entries?.ToList() ?? new List<Entry>();
            entryList.Add(newEntry);
            Entries = entryList.ToArray();
            slotCount = Entries.Length;
        }

        public void RemoveEntry(int index)
        {
            if (Entries == null || index < 0 || index >= Entries.Length)
                throw new ArgumentOutOfRangeException(nameof(index), "Invalid index");

            List<Entry> entryList = Entries.ToList();
            entryList.RemoveAt(index);
            Entries = entryList.ToArray();
            slotCount = Entries.Length;
        }
    }
}
