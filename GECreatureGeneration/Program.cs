using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GECreatureGeneration
{
    class Program
    {
        private Creature BestCreature;
        private Creature LowestCreature;
        private float Years;
        private float YearCleaner;
        private Random RNG = SingleTon.GetRNG();
        private int StartingPoolSize;
        private int CurrentGeneration = 0;
        private int GenerationToSimulate;
        private float AverageFitness;
        private long MaxPopulation;
        private long TotalPopulation;
        private float MutationRate;
        private static List<Creature> Creatures;
        private float HighestFitness;
        private float LowestFitness;

        #region Stats
        int TempHerb;
        int TempCarn;
        int TempOmni;

        int TempSpecies0;
        int TempSpecies1;
        int TempSpecies2;
        int TempSpecies3;
        int TempSpecies4;
        int TempSpecies5;
        int TempSpecies6;
        int TempSpecies7;
        int TempSpecies8;
        int TempSpecies9;
        #endregion

        #region InputChecker
        private string GenerationToSimulateInputString;
        private string MaxPopulationInputString;
        private string MutationRateInputString;
        private string StartingPoolStringInput;
        #endregion

        static void Main(string[] args) { new Program().Start(); }

        public void Start()
        {
            Console.Title = "GECreatureGeneration";
            Console.WriteLine("Welcome to this demo of genetic evolution");
            Console.WriteLine("there is a few parameters you need to fill out before we begin");
            Console.Write("size of starting pool: ");
            StartingPoolStringInput = Console.ReadLine();
            if (!(int.TryParse(StartingPoolStringInput, out StartingPoolSize)))
            {
                StartingPoolSize = (int)Redo();
            }
            Console.Clear();
            Console.Write("Max population size: ");
            MaxPopulationInputString = Console.ReadLine();
            if (!(long.TryParse(MaxPopulationInputString, out MaxPopulation)))
            {
                MaxPopulation = Redo();
            }
            Console.Clear();
            Console.Write("Mutation rate: ");
            MutationRateInputString = Console.ReadLine();
            if (!(float.TryParse(MutationRateInputString, out MutationRate)))
            {
                MutationRate = RedoFloat();
            }
            Console.Clear();
            Console.Write("what limit to generations: ");
            GenerationToSimulateInputString = Console.ReadLine();
            if (!(int.TryParse(GenerationToSimulateInputString, out GenerationToSimulate)))
            {
                GenerationToSimulate = (int)Redo();
            }
            SingleTon.SetMutationRate(MutationRate);
            Creatures = new List<Creature>();
            TotalPopulation = Creatures.Count;
            SingleTon.SetMaxPopulation((int)MaxPopulation);
            SingleTon.SetTotalPopulation((int)TotalPopulation);
            CreateInitialCreattures();
            BestCreature = Creatures[0];
            LowestCreature = Creatures[0];
            while (GenerationToSimulate != CurrentGeneration)
            {
                Console.Clear();
                Years += 0.1f;
                YearCleaner += 0.1f;
                SimulateLife();
                DrawVisuals();
                if (YearCleaner >= 1)
                {
                    if (TotalPopulation > (MaxPopulation * 0.25))
                    {
                        if (RNG.Next(1, 101) < 1)
                        {
                            Creatures.Remove(LowestCreature);
                        }
                    }
                }
                if(Console.ReadKey().Key == ConsoleKey.R)
                {
                    Start();
                }
                
            }
            Console.Beep();
            Thread.Sleep(3000);
            Console.ReadKey();

        }


        private void DrawVisuals()
        {
            Console.WriteLine("Years: " + Years.ToString("0.0"));
            Console.WriteLine("Generation: " + CurrentGeneration + "/" + GenerationToSimulate);
            Console.WriteLine("Population: " + Creatures.Count + "/" + MaxPopulation);
            Console.WriteLine("Mutation rate: " + MutationRate);
            Console.WriteLine("Average fitness: " + AverageFitness.ToString("0.0"));
            Console.WriteLine("Highest fitness: " + HighestFitness.ToString("0.0"));
            Console.WriteLine("Lowest fitness: " + LowestFitness.ToString("0.0"));
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Best Creature");
            Console.WriteLine("DNA: " + BestCreature.GetDNA()[0] + "|"+ BestCreature.GetDNA()[1] + "|" + BestCreature.GetDNA()[2] + "|" + BestCreature.GetDNA()[3] + "|" + BestCreature.GetDNA()[4] + "|" + BestCreature.GetDNA()[5] + "|" + BestCreature.GetDNA()[6]);
            Console.WriteLine("Breedable: " + BestCreature.CanBreed);
            Console.WriteLine("Food: " + BestCreature.StomachFuldness.ToString("0.0"));
            Console.WriteLine("Food type " + BestCreature.GetFoodType());
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Worst Creature");
            Console.WriteLine("DNA: " + LowestCreature.GetDNA()[0] + "|" + BestCreature.GetDNA()[1] + "|" + BestCreature.GetDNA()[2] + "|" + BestCreature.GetDNA()[3] + "|" + BestCreature.GetDNA()[4] + "|" + BestCreature.GetDNA()[5] + "|" + BestCreature.GetDNA()[6]);
            Console.WriteLine("Breedable: " + LowestCreature.CanBreed);
            Console.WriteLine("Food: " + LowestCreature.StomachFuldness.ToString("0.0"));
            Console.WriteLine("Food type: " + LowestCreature.GetFoodType());
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Food types");
            Console.WriteLine("Carnivores: " + TempCarn);
            Console.WriteLine("Herbivores: " + TempHerb);
            Console.WriteLine("Omnivores: " + TempOmni);
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Species");
            Console.WriteLine("Species 0: " + TempSpecies0);
            Console.WriteLine("Species 1: " + TempSpecies1);
            Console.WriteLine("Species 2: " + TempSpecies2);
            Console.WriteLine("Species 3: " + TempSpecies3);
            Console.WriteLine("Species 4: " + TempSpecies4);
            Console.WriteLine("Species 5: " + TempSpecies5);
            Console.WriteLine("Species 6: " + TempSpecies6);
            Console.WriteLine("Species 7: " + TempSpecies7);
            Console.WriteLine("Species 8: " + TempSpecies8);
            Console.WriteLine("Species 9: " + TempSpecies9);
        }

        private void CreateInitialCreattures()
        {
            CurrentGeneration++;
            for (int i = 0; i < StartingPoolSize; i++)
            {
                Creatures.Add(new Creature(new int[] { 1, RNG.Next(0, 10), RNG.Next(1, 4), RNG.Next(5, 16), RNG.Next(10, 21), RNG.Next(2, 6), RNG.Next(10, 20) }));
            }
        }

        private void SimulateLife()
        {
            #region Stats reset
            TempOmni = 0;
            TempCarn = 0;
            TempHerb = 0;
            TempSpecies0 = 0;
            TempSpecies1 = 0;
            TempSpecies2 = 0;
            TempSpecies3 = 0;
            TempSpecies4 = 0;
            TempSpecies5 = 0;
            TempSpecies6 = 0;
            TempSpecies7 = 0;
            TempSpecies8 = 0;
            TempSpecies9 = 0;
            AverageFitness = 0;
            try
            {
                LowestFitness = Creatures[0].GetFitness();
                HighestFitness = Creatures[0].GetFitness();
            }
            catch { };
            #endregion

            for (int i = 0; i < Creatures.Count; i++)
            {
                Creatures[i].Live();
                if (Creatures[i].Dead == true)
                {
                    Creatures.Remove(Creatures[i]);
                }
                else
                {
                    #region Stats collecting
                    if (CurrentGeneration < Creatures[i].GetGeneration())
                    {
                        CurrentGeneration = Creatures[i].GetGeneration();
                    }
                    AverageFitness += Creatures[i].GetFitness();
                    if (Creatures[i].GetFitness() > HighestFitness)
                    {
                        HighestFitness = Creatures[i].GetFitness();
                        BestCreature = Creatures[i];
                    }
                    if (Creatures[i].GetFitness() < LowestFitness)
                    {
                        LowestFitness = Creatures[i].GetFitness();
                        LowestCreature = Creatures[i];
                    }


                    switch(Creatures[i].GetFoodType())
                    {
                        case ConsumeType.Carnivore:
                            TempCarn++;
                            break;

                        case ConsumeType.Herbivore:
                            TempHerb++;
                            break;

                        case ConsumeType.Omnivore:

                            TempOmni++;
                            break;
                    }

                    switch(Creatures[i].GetSpecies())
                    {
                        case 0:
                            TempSpecies0++;
                            break;

                        case 1:
                            TempSpecies1++;
                            break;

                        case 2:
                            TempSpecies2++;
                            break;

                        case 3:
                            TempSpecies3++;
                            break;

                        case 4:
                            TempSpecies4++;
                            break;

                        case 5:
                            TempSpecies5++;
                            break;

                        case 6:
                            TempSpecies6++;
                            break;

                        case 7:
                            TempSpecies7++;
                            break;

                        case 8:
                            TempSpecies8++;
                            break;

                        case 9:
                            TempSpecies9++;
                            break;

                    }
                    #endregion
                }

            }
            SingleTon.SetCarivoreAliveData(TempCarn);
            SingleTon.SetHerbivoreAliveData(TempHerb);
            SingleTon.SetOmnivoreAliveData(TempOmni);
            TotalPopulation = Creatures.Count;
            AverageFitness /= Creatures.Count;

            //try and breed
            if (TotalPopulation <= MaxPopulation)
            {
                for (int i = 0; i < Creatures.Count; i++)
                {
                    if(Creatures[i].CanBreed)
                    {
                        for(int x = 0; x < Creatures.Count; x++)
                        {
                            if (TotalPopulation <= MaxPopulation)
                            {
                                if (Creatures[x] == Creatures[i] && Creatures[x].CanBreed && Creatures[x].GetSpecies() == Creatures[i].GetSpecies())
                                {
                                    Creatures.Add(new Creature(Creatures[x].GetDNAForBreeding(), Creatures[i].GetDNAForBreeding()));
                                }
                            }
                        }
                    }
                }
            }
        }

        public List<Creature> GetCreatureList()
        {
            return Creatures;
        }

        private long Redo()
        {
            long ReturnValue;
            Console.Clear();
            Console.WriteLine("Please give a valid input in form of a number");
            if (!(long.TryParse(Console.ReadLine(), out ReturnValue)))
            {
                ReturnValue = Redo();
            }
            return ReturnValue;
        }

        private float RedoFloat()
        {
            float ReturnValue;
            Console.Clear();
            Console.WriteLine("Please give a valid input in form of a number");
            if (!(float.TryParse(Console.ReadLine(), out ReturnValue)))
            {
                ReturnValue = RedoFloat();
            }
            return ReturnValue;
        }

    }
}
