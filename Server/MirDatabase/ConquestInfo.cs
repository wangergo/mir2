﻿using Server.MirEnvir;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Server.MirDatabase
{
    public class ConquestInfo
    {
        [Key]
        public int Index { get; set; }
        [NotMapped]
        public bool FullMap;
        public Point Location;
        public string DBLocation
        {
            get { return Location.X + "," + Location.Y; }
            set
            {
                var tempArray = value.Split(',');
                if (tempArray.Length != 2)
                {
                    Location.X = 0;
                    Location.Y = 0;
                }
                else
                {
                    int result = 0;
                    int.TryParse(tempArray[0], out result);
                    Location.X = result;
                    int.TryParse(tempArray[1], out result);
                    Location.Y = result;
                }
            }
        }
        public ushort Size;
        public int DBSize { get { return Size; } set { Size = (ushort) value; } }
        public string Name { get; set; }
        public int MapIndex { get; set; }
        public int PalaceIndex { get; set; }
        public List<int> ExtraMaps = new List<int>();

        public string DBExtraMaps
        {
            get { return string.Join(",", ExtraMaps); }
            set
            {
                ExtraMaps = string.IsNullOrEmpty(value) ? new List<int>() : value.Split(',').Select(int.Parse).ToList();
            }
        }

        public List<ConquestArcherInfo> ConquestGuards = new List<ConquestArcherInfo>();
        public List<ConquestGateInfo> ConquestGates = new List<ConquestGateInfo>();
        public List<ConquestWallInfo> ConquestWalls = new List<ConquestWallInfo>();
        public List<ConquestSiegeInfo> ConquestSieges = new List<ConquestSiegeInfo>();
        public List<ConquestFlagInfo> ConquestFlags = new List<ConquestFlagInfo>();
        public int GuardIndex { get; set; }
        public int GateIndex { get; set; }
        public int WallIndex { get; set; }
        public int SiegeIndex { get; set; }
        public int FlagIndex;

        public byte StartHour { get; set; } = 0;
        public int WarLength { get; set; } = 60;

        private int counter;

        public ConquestType Type { get; set; } = ConquestType.Request;
        public ConquestGame Game { get; set; } = ConquestGame.CapturePalace;

        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
        public bool Sunday { get; set; }


//King of the hill
        public Point KingLocation;

        public string DBKingLocation
        {
            get { return KingLocation.X + "," + KingLocation.Y; }
            set
            {
                var tempArray = value.Split(',');
                if (tempArray.Length != 2)
                {
                    KingLocation.X = 0;
                    KingLocation.Y = 0;
                }
                else
                {
                    int result = 0;
                    int.TryParse(tempArray[0], out result);
                    KingLocation.X = result;
                    int.TryParse(tempArray[1], out result);
                    KingLocation.Y = result;
                }
            }
        }
        public ushort KingSize;
        public int DBKingSize { get { return KingSize; } set { KingSize = (ushort) value; } }
        public Point ObjectLoc;
        public string DBObjectLoc
        {
            get { return ObjectLoc.X + "," + ObjectLoc.Y; }
            set
            {
                var tempArray = value.Split(',');
                if (tempArray.Length != 2)
                {
                    ObjectLoc.X = 0;
                    ObjectLoc.Y = 0;
                }
                else
                {
                    int result = 0;
                    int.TryParse(tempArray[0], out result);
                    ObjectLoc.X = result;
                    int.TryParse(tempArray[1], out result);
                    ObjectLoc.Y = result;
                }
            }
        }
        public ushort ObjectSize;
        public int DBObjectSize { get { return ObjectSize;} set { ObjectSize = (ushort) value; } }

        //Control points
        public List<ConquestFlagInfo> ControlPoints = new List<ConquestFlagInfo>();
        public int ControlPointIndex;

        public ConquestInfo()
        {

        }

        public ConquestInfo(BinaryReader reader)
        {
            Index = reader.ReadInt32();

            if(Envir.LoadVersion > 73)
            {
                FullMap = reader.ReadBoolean();
            }

            Location = new Point(reader.ReadInt32(), reader.ReadInt32());
            Size = reader.ReadUInt16();
            Name = reader.ReadString();
            MapIndex = reader.ReadInt32();
            PalaceIndex = reader.ReadInt32();
            GuardIndex = reader.ReadInt32();
            GateIndex = reader.ReadInt32();
            WallIndex = reader.ReadInt32();
            SiegeIndex = reader.ReadInt32();

            if (Envir.LoadVersion > 72)
            {
                FlagIndex = reader.ReadInt32();
            }

            counter = reader.ReadInt32();
            for (int i = 0; i < counter; i++)
            {
                ConquestGuards.Add(new ConquestArcherInfo(reader));
            }
            counter = reader.ReadInt32();
            for (int i = 0; i < counter; i++)
            {
                ExtraMaps.Add(reader.ReadInt32());
            }
            counter = reader.ReadInt32();
            for (int i = 0; i < counter; i++)
            {
                ConquestGates.Add(new ConquestGateInfo(reader));
            }
            counter = reader.ReadInt32();
            for (int i = 0; i < counter; i++)
            {
                ConquestWalls.Add(new ConquestWallInfo(reader));
            }
            counter = reader.ReadInt32();
            for (int i = 0; i < counter; i++)
            {
                ConquestSieges.Add(new ConquestSiegeInfo(reader));
            }

            if (Envir.LoadVersion > 72)
            {
                counter = reader.ReadInt32();
                for (int i = 0; i < counter; i++)
                {
                    ConquestFlags.Add(new ConquestFlagInfo(reader));
                }
            }

            StartHour = reader.ReadByte();
            WarLength = reader.ReadInt32();
            Type = (ConquestType)reader.ReadByte();
            Game = (ConquestGame)reader.ReadByte();

            Monday = reader.ReadBoolean();
            Tuesday = reader.ReadBoolean();
            Wednesday = reader.ReadBoolean();
            Thursday = reader.ReadBoolean();
            Friday = reader.ReadBoolean();
            Saturday = reader.ReadBoolean();
            Sunday = reader.ReadBoolean();

            KingLocation = new Point(reader.ReadInt32(), reader.ReadInt32());
            KingSize = reader.ReadUInt16();

            if (Envir.LoadVersion > 74)
            {
                ControlPointIndex = reader.ReadInt32();
                counter = reader.ReadInt32();
                for (int i = 0; i < counter; i++)
                {
                    ControlPoints.Add(new ConquestFlagInfo(reader));
        }
            }
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(Index);
            writer.Write(FullMap);
            writer.Write(Location.X);
            writer.Write(Location.Y);
            writer.Write(Size);
            writer.Write(Name);
            writer.Write(MapIndex);
            writer.Write(PalaceIndex);
            writer.Write(GuardIndex);
            writer.Write(GateIndex);
            writer.Write(WallIndex);
            writer.Write(SiegeIndex);
            writer.Write(FlagIndex);

            writer.Write(ConquestGuards.Count);
            for (int i = 0; i < ConquestGuards.Count; i++)
            {
                ConquestGuards[i].Save(writer);
            }
            writer.Write(ExtraMaps.Count);
            for (int i = 0; i < ExtraMaps.Count; i++)
            {
                writer.Write(ExtraMaps[i]);
            }
            writer.Write(ConquestGates.Count);
            for (int i = 0; i < ConquestGates.Count; i++)
            {
                ConquestGates[i].Save(writer);
            }
            writer.Write(ConquestWalls.Count);
            for (int i = 0; i < ConquestWalls.Count; i++)
            {
                ConquestWalls[i].Save(writer);
            }
            writer.Write(ConquestSieges.Count);
            for (int i = 0; i < ConquestSieges.Count; i++)
            {
                ConquestSieges[i].Save(writer);
            }

            writer.Write(ConquestFlags.Count);
            for (int i = 0; i < ConquestFlags.Count; i++)
            {
                ConquestFlags[i].Save(writer);
            }
            writer.Write(StartHour);
            writer.Write(WarLength);
            writer.Write((byte)Type);
            writer.Write((byte)Game);

            writer.Write(Monday);
            writer.Write(Tuesday);
            writer.Write(Wednesday);
            writer.Write(Thursday);
            writer.Write(Friday);
            writer.Write(Saturday);
            writer.Write(Sunday);

            writer.Write(KingLocation.X);
            writer.Write(KingLocation.Y);
            writer.Write(KingSize);

            writer.Write(ControlPointIndex);
            writer.Write(ControlPoints.Count);
            for (int i = 0; i < ControlPoints.Count; i++)
            {
                ControlPoints[i].Save(writer);
            }

        }

        public override string ToString()
        {
            return string.Format("{0}- {1}", Index, Name);
        }
    }

    public class ConquestSiegeInfo
    {
        [Key]
        public int Index { get; set; }
        public Point Location;
        public string DBLocation
        {
            get { return Location.X + "," + Location.Y; }
            set
            {
                var tempArray = value.Split(',');
                if (tempArray.Length != 2)
                {
                    Location.X = 0;
                    Location.Y = 0;
                }
                else
                {
                    int result = 0;
                    int.TryParse(tempArray[0], out result);
                    Location.X = result;
                    int.TryParse(tempArray[1], out result);
                    Location.Y = result;
                }
            }
        }
        public int ConquestInfoIndex { get; set; }
        public int MobIndex { get; set; }
        public string Name { get; set; }
        public uint RepairCost;
        public long DBRepairCost { get { return RepairCost;} set { RepairCost = (uint) value; } }

        public ConquestSiegeInfo()
        {

        }

        public ConquestSiegeInfo(BinaryReader reader)
        {
            Index = reader.ReadInt32();
            Location = new Point(reader.ReadInt32(), reader.ReadInt32());
            MobIndex = reader.ReadInt32();
            Name = reader.ReadString();
            RepairCost = reader.ReadUInt32();
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(Index);
            writer.Write(Location.X);
            writer.Write(Location.Y);
            writer.Write(MobIndex);
            writer.Write(Name);
            writer.Write(RepairCost);
        }

        public override string ToString()
        {
            return string.Format("{0} - {1} ({2})", Index, Name, Location);
        }


    }

    public class ConquestWallInfo
    {
        [Key]
        public int Index { get; set; }
        public Point Location;
        public string DBLocation
        {
            get { return Location.X + "," + Location.Y; }
            set
            {
                var tempArray = value.Split(',');
                if (tempArray.Length != 2)
                {
                    Location.X = 0;
                    Location.Y = 0;
                }
                else
                {
                    int result = 0;
                    int.TryParse(tempArray[0], out result);
                    Location.X = result;
                    int.TryParse(tempArray[1], out result);
                    Location.Y = result;
                }
            }
        }
        public int ConquestInfoIndex { get; set; }
        public int MobIndex { get; set; }
        public string Name { get; set; }
        public uint RepairCost;
        public long DBRepairCost { get { return RepairCost; } set { RepairCost = (uint)value; } }

        public ConquestWallInfo()
        {

        }

        public ConquestWallInfo(BinaryReader reader)
        {
            Index = reader.ReadInt32();
            Location = new Point(reader.ReadInt32(), reader.ReadInt32());
            MobIndex = reader.ReadInt32();
            Name = reader.ReadString();
            RepairCost = reader.ReadUInt32();
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(Index);
            writer.Write(Location.X);
            writer.Write(Location.Y);
            writer.Write(MobIndex);
            writer.Write(Name);
            writer.Write(RepairCost);
        }

        public override string ToString()
        {
            return string.Format("{0} - {1} ({2})", Index, Name, Location);
        }


    }

    public class ConquestGateInfo
    {
        [Key]
        public int Index { get; set; }
        public Point Location;
        public string DBLocation
        {
            get { return Location.X + "," + Location.Y; }
            set
            {
                var tempArray = value.Split(',');
                if (tempArray.Length != 2)
                {
                    Location.X = 0;
                    Location.Y = 0;
                }
                else
                {
                    int result = 0;
                    int.TryParse(tempArray[0], out result);
                    Location.X = result;
                    int.TryParse(tempArray[1], out result);
                    Location.Y = result;
                }
            }
        }
        public int ConquestInfoIndex { get; set; }
        public int MobIndex { get; set; }
        public string Name { get; set; }
        public uint RepairCost;
        public long DBRepairCost { get { return RepairCost; } set { RepairCost = (uint)value; } }

        public ConquestGateInfo()
        {

        }

        public ConquestGateInfo(BinaryReader reader)
        {
            Index = reader.ReadInt32();
            Location = new Point(reader.ReadInt32(), reader.ReadInt32());
            MobIndex = reader.ReadInt32();
            Name = reader.ReadString();
            RepairCost = reader.ReadUInt32();
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(Index);
            writer.Write(Location.X);
            writer.Write(Location.Y);
            writer.Write(MobIndex);
            writer.Write(Name);
            writer.Write(RepairCost);
        }

        public override string ToString()
        {
            return string.Format("{0} - {1} ({2})", Index, Name, Location);
        }


    }

    public class ConquestArcherInfo
    {
        [Key]
        public int Index { get; set; }
        public Point Location;
        public string DBLocation
        {
            get { return Location.X + "," + Location.Y; }
            set
            {
                var tempArray = value.Split(',');
                if (tempArray.Length != 2)
                {
                    Location.X = 0;
                    Location.Y = 0;
                }
                else
                {
                    int result = 0;
                    int.TryParse(tempArray[0], out result);
                    Location.X = result;
                    int.TryParse(tempArray[1], out result);
                    Location.Y = result;
                }
            }
        }
        public int ConquestInfoIndex { get; set; }
        public int MobIndex { get; set; }
        public string Name { get; set; }
        public uint RepairCost;
        public long DBRepairCost { get { return RepairCost; } set { RepairCost = (uint)value; } }

        public ConquestArcherInfo()
        {

        }

        public ConquestArcherInfo(BinaryReader reader)
        {
            Index = reader.ReadInt32();
            Location = new Point(reader.ReadInt32(), reader.ReadInt32());
            MobIndex = reader.ReadInt32();
            Name = reader.ReadString();
            RepairCost = reader.ReadUInt32();
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(Index);
            writer.Write(Location.X);
            writer.Write(Location.Y);
            writer.Write(MobIndex);
            writer.Write(Name);
            writer.Write(RepairCost);
        }

        public override string ToString()
        {
            return string.Format("{0} - {1} ({2})", Index, Name, Location);
        }


    }

    public class ConquestFlagInfo
    {
        [Key]
        public int Index { get; set; }
        public Point Location;

        public string DBLocation
        {
            get { return Location.X + "," + Location.Y; }
            set
            {
                var tempArray = value.Split(',');
                if (tempArray.Length != 2)
                {
                    Location.X = 0;
                    Location.Y = 0;
                }
                else
                {
                    int result = 0;
                    int.TryParse(tempArray[0], out result);
                    Location.X = result;
                    int.TryParse(tempArray[1], out result);
                    Location.Y = result;
                }
            }
        }
        public string Name { get; set; }
        public string FileName { get; set; } = string.Empty;
        public int ConquestInfoIndex { get; set; }
        public ConquestFlagType ConquestFlagType { get; set; } = ConquestFlagType.Flag;
        public ConquestFlagInfo()
        {

        }

        public ConquestFlagInfo(BinaryReader reader)
        {
            Index = reader.ReadInt32();
            Location = new Point(reader.ReadInt32(), reader.ReadInt32());
            Name = reader.ReadString();
            FileName = reader.ReadString();
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(Index);
            writer.Write(Location.X);
            writer.Write(Location.Y);
            writer.Write(Name);
            writer.Write(FileName);
        }

        public override string ToString()
        {
            return string.Format("{0} - {1} ({2})", Index, Name, Location);
        }
    }

    public enum ConquestFlagType
    {
        Flag,
        Control_Point
    }
}
