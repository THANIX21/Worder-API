﻿using WorderAPI.Classes.Interfaces;

namespace WorderAPI.Classes.Base
{
    public class Word : IWord
    {
        public int ID { get; set; }
        public string Term  { get; set; }
        public DateTime DTCreated { get; set; }
        public DateTime DTAltered { get; set; }
        public int Type { get; set; }
    }
}
