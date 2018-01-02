using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GECreatureGeneration
{
    static class SingleTon
    {
        private static Random RNGInstace;
        private static float MutationrateInstance;
        private static int HerbivoresAliveData = 1;
        private static int CarnivoresAliveData = 1;
        private static int OmnivoresAliveData = 1;
        private static int totalpopulation;
        private static int MaxPopulation;

        public static Random GetRNG()
        {
            if(RNGInstace == null)
            {
                RNGInstace = new Random();
            }

            return RNGInstace;
        }

        public static float GetMutationInstance()
        {
            return MutationrateInstance;
        }

        public static void SetMutationRate(float mutationrate)
        {
            MutationrateInstance = mutationrate;
        }

        public static int GetHerbivoresAliveData()
        {
            return HerbivoresAliveData;
        }
        public static int GetCarnivoresAliveData()
        {
            return CarnivoresAliveData;
        }
        public static int GetOmnivoresAliveData()
        {
            return OmnivoresAliveData;
        }

        public static void AssignCreaturesFoodtype(int herb, int carn, int omni)
        {
            HerbivoresAliveData = herb;
            CarnivoresAliveData = carn;
            OmnivoresAliveData = omni;
        }

        public static void SetMaxPopulation(int Max)
        {
            MaxPopulation = Max;
        }

        public static void SetTotalPopulation(int total)
        {
            totalpopulation = total;
        }

        public static int GetMaxPopulation()
        {
            return MaxPopulation;
        }

        public static int GetTotalPopulation()
        {
            return totalpopulation;
        }

        public static void SetCarivoreAliveData(int alive)
        {
            CarnivoresAliveData = alive;
        }

        public static void SetHerbivoreAliveData(int alive)
        {
            HerbivoresAliveData = alive;
        }

        public static void SetOmnivoreAliveData(int alive)
        {
            OmnivoresAliveData = alive;
        }
    }
}
