using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GECreatureGeneration
{
    class Creature
    {
        private Program GetCreatureList = new Program();
        private Creature CreatureAttackingToEat;
        private Random RNG = SingleTon.GetRNG();
        private int Generation;
        private int[] DNA;
        private ConsumeType Foodtype;
        private short Species;
        private float HungerRate;
        private float StomachSize;
        public float StomachFuldness;
        private int Attack;
        private int Health;
        private int CurrentHealth;
        public bool CanBreed = false;
        private float Age = 0; // make event
        public bool Dead = false;
        private int BreedCounter;

        public Creature(int[] dna)
        {
            DNA = dna;
            Generation = DNA[0];
            Species = (short)DNA[1];
            switch (DNA[2])
            {
                case 1:
                    Foodtype = ConsumeType.Carnivore;
                    break;

                case 2:
                    Foodtype = ConsumeType.Herbivore;
                    break;

                case 3:
                    Foodtype = ConsumeType.Omnivore;
                    break;
            }
            HungerRate = ((float)DNA[3] / 10);
            StomachSize = DNA[4];
            Attack = DNA[5];
            Health = DNA[6];
            CurrentHealth = Health;
            StomachFuldness = StomachFuldness * 0.2f;
        }

        public ConsumeType GetFoodType()
        {
            return Foodtype;
        }
        //setup condetions for breeding
        //hunger on rotations

        public Creature(int[] dna1, int[] dna2)
        {
            //perform dna splice and mutations
            //selection
            DNA = new int[dna1.Length];
            for (int i = 0; i < dna1.Length; i++)
            {
                if (RNG.Next(1, 1001) > 50)
                {
                    DNA[i] = dna1[i];
                }
                else
                {
                    DNA[i] = dna2[i];
                }
            }
            //mutation
            for (int i = 0; i < DNA.Length; i++)
            {
                if (RNG.Next(1, 101) <= (SingleTon.GetMutationInstance() * 100))//use mutation rate here
                {
                    switch (i)
                    {
                        case 2:
                            DNA[2] = RNG.Next(1, 4);
                            break;

                        case 3:
                            DNA[3] = RNG.Next((int)((float)(DNA[3] - ((float)DNA[3] * SingleTon.GetMutationInstance()))), (int)((float)(DNA[3] + (float)(DNA[3] * (float)SingleTon.GetMutationInstance()))));
                            break;

                        case 4:
                            DNA[4] = RNG.Next((int)((float)(DNA[4] - ((float)DNA[4] * SingleTon.GetMutationInstance()))), (int)((float)(DNA[4] + (float)(DNA[4] * (float)SingleTon.GetMutationInstance()))));
                            break;

                        case 5:
                            DNA[5] = RNG.Next((int)((float)(DNA[5] - ((float)DNA[5] * SingleTon.GetMutationInstance()))), (int)((float)(DNA[5] + (float)(DNA[5] * (float)SingleTon.GetMutationInstance()))));
                            break;

                        case 6:
                            DNA[6] = RNG.Next((int)((float)(DNA[6] - ((float)DNA[6] * SingleTon.GetMutationInstance()))), (int)((float)(DNA[6] + (float)(DNA[6] * (float)SingleTon.GetMutationInstance()))));
                            break;
                    }
                }
            }
            DNA[0]++;
            Generation = DNA[0];
            Species = (short)DNA[1];
            switch (DNA[2])
            {
                case 1:
                    Foodtype = ConsumeType.Carnivore;
                    break;

                case 2:
                    Foodtype = ConsumeType.Herbivore;
                    break;

                case 3:
                    Foodtype = ConsumeType.Omnivore;
                    break;
            }
            HungerRate = ((float)DNA[3] / 10);
            StomachSize = DNA[4];
            Attack = DNA[5];
            Health = DNA[6];
            CurrentHealth = Health;


        }

        public int[] GetDNA()
        {
            return DNA;
        }

        public void GetAttacked(int attack)
        {
            Health -= attack;
            if (Health < 0)
            {
                Health = 0;
            }
        }

        public void Live()
        {
            Age += 0.1f;
            if (StomachFuldness < (StomachSize * 0.7))
            {
                //hunt / eat   | the more there is the less plant matter there is, the less herbivore there is the less food for carnivores
                switch (Foodtype)
                {
                    case ConsumeType.Carnivore:
                        if (CreatureAttackingToEat != null)
                        {
                            try
                            {
                                CreatureAttackingToEat.GetAttacked(Attack);
                                if (CreatureAttackingToEat.GetHealth() == 0)
                                {
                                    CreatureAttackingToEat = null;
                                    StomachFuldness += (StomachFuldness * 0.5f);
                                }
                            }
                            catch
                            {
                                CreatureAttackingToEat = null;
                            }
                        }
                        else
                        {
                            float foodChances;
                            foodChances = RNG.Next(1, 101);
                            if ((foodChances / 100) < ((float)SingleTon.GetHerbivoresAliveData() / (float)SingleTon.GetTotalPopulation()))
                            {
                                Creature PickedCreature;
                                while (CreatureAttackingToEat == null)
                                {
                                    PickedCreature = GetCreatureList.GetCreatureList()[RNG.Next(0, GetCreatureList.GetCreatureList().Count)];
                                    if (PickedCreature.GetFoodType() == ConsumeType.Herbivore)
                                    {
                                        CreatureAttackingToEat = PickedCreature;
                                    }
                                }
                                CreatureAttackingToEat.GetAttacked(Attack);
                                if (CreatureAttackingToEat.GetHealth() == 0)
                                {
                                    CreatureAttackingToEat = null;
                                    StomachFuldness += (StomachFuldness * 0.5f);
                                }

                            }
                        }
                        break;

                    case ConsumeType.Herbivore:
                        //20% chance to find food
                        float ChanceTofindfood = 40;
                        if (((((float)SingleTon.GetHerbivoresAliveData() * 2) + (float)SingleTon.GetOmnivoresAliveData()) / (float)SingleTon.GetMaxPopulation()) > 0.5)
                        {
                            ChanceTofindfood = ChanceTofindfood - (ChanceTofindfood * (((((float)SingleTon.GetHerbivoresAliveData() * 2) + (float)SingleTon.GetOmnivoresAliveData()) / (float)SingleTon.GetMaxPopulation()) - 0.5f));
                        }
                        //ChanceTofindfood

                        if (RNG.Next(1, 101) <= ChanceTofindfood)
                        {
                            //add 2 point to herb pool
                            StomachFuldness += 2; //temp
                        }
                        break;

                    case ConsumeType.Omnivore:
                        //30% chance to find food
                        if (RNG.Next(1, 101) <= 50)
                        {
                            //1 point to herb pool
                            //chance of meat or herb   = 20/80
                            StomachFuldness += 1;//temp
                        }
                        break;
                }

            }
            StomachFuldness -= HungerRate;
            if (StomachFuldness < 0)
            {
                StomachFuldness = 0;
            }
            if (StomachFuldness == 0)
            {
                CurrentHealth--;
            }

            if (CurrentHealth == 0)
            {
                Dead = true;
            }


            if (StomachFuldness >= (StomachSize * 0.5) && CurrentHealth > (Health * 0.5) && Age > 5)
            {
                CanBreed = true;
            }
            else
            {
                CanBreed = false;
            }
        }

        public int GetHealth()
        {
            return Health;
        }

        public int[] GetDNAForBreeding()
        {
            StomachFuldness -= (StomachFuldness * 0.5f);
            CanBreed = false;
            BreedCounter++;
            if (BreedCounter >= 5)
            {
                Dead = true;
            }
            return DNA;
        }

        public int GetGeneration()
        {
            return Generation;
        }

        public short GetSpecies()
        {
            return Species;
        }

        public float GetFitness()
        {
            return (Age + (Health * (StomachFuldness / StomachSize))) * BreedCounter;
        }
    }

    public enum ConsumeType
    {
        Carnivore, Herbivore, Omnivore
    }
}
