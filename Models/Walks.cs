using System;
using System.Collections.Generic;
using System.Data;
using System.Text;


namespace LordsOfDogtown.Models
{
    public class Walks
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime Date { get; set; }
        
        public int Duration { get; set; }
        
        public int DogId { get; set; }

        public Dog Dog { get; set; }

        public int WalkerId { get; set; }

        public Walker Walker { get; set; }





    }
}