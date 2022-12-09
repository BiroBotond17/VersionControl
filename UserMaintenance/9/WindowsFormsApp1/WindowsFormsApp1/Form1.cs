using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.Entities;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        List<Person> Population = new List<Person>();
        List<BirthProbability> BirthProbabilities = new List<BirthProbability>();
        List<DeathProbability> DeathProbabilities = new List<DeathProbability>();
        Random rng = new Random(1234);
        List<Person> nokszama;
        List<Person> ferfiakszama;
        public Form1()
        {
            InitializeComponent();
            Population = GetPopulation(@"C:\Temp\nép.csv");
            BirthProbabilities = GetBirth(@"C:\Temp\születés.csv");
            DeathProbabilities = GetDeath(@"C:\Temp\halál.csv");
            
            numericUpDown1.Minimum = 2006;
            numericUpDown1.Maximum = 2800;

        }

        public void Simulation()
        {
            for (int year = 2005; year <= numericUpDown1.Value; year++)
            {
               
                for (int i = 0; i < Population.Count; i++)
                {
                    SimStep(year, Population[i]);
                }

                int nbrOfMales = (from x in Population
                                  where x.Gender == Gender.Male && x.IsAlive
                                  select x).Count();
                int nbrOfFemales = (from x in Population
                                    where x.Gender == Gender.Female && x.IsAlive
                                    select x).Count();
                richTextBox1.Text=string.Format("Szimulációs év:{0}\n\tfiúk:{1}\n\tlányok{2}\n", year, nbrOfMales, nbrOfFemales);
                
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        public List<Person> GetPopulation(string csvpath)
        {
            List<Person> population = new List<Person>();

            using (StreamReader sr = new StreamReader(csvpath, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split(';');
                    population.Add(new Person()
                    {
                        BirthYear = int.Parse(line[0]),
                        Gender = (Gender)Enum.Parse(typeof(Gender), line[1]),
                        NbrOfChildren = int.Parse(line[2])
                    });
                }
            }

            return population;
        }
        public List<BirthProbability> GetBirth(string csvpath)
        {
            List<BirthProbability> birthProbabilities = new List<BirthProbability>();

            using (StreamReader sr = new StreamReader(csvpath, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split(';');
                    birthProbabilities.Add(new BirthProbability()
                    {
                        Kor = int.Parse(line[0]),
                        NbrOfChildren= int.Parse(line[1]),
                        valsz=double.Parse(line[2])

                    });
                }
            }

            return birthProbabilities;
        }
        public List<DeathProbability> GetDeath(string csvpath)
        {
            List<DeathProbability> deathProbabilities = new List<DeathProbability>();

            using (StreamReader sr = new StreamReader(csvpath, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split(';');
                    deathProbabilities.Add(new DeathProbability()
                    {
                        Gender= (Gender)Enum.Parse(typeof(Gender), line[0]),
                        kor = int.Parse(line[1]),
                        halalvalsz= double.Parse(line[2])
                    });
                }
            }

            return deathProbabilities;
        }
        private void SimStep(int year, Person person)
        {
            
            if (!person.IsAlive) return;

            
            byte age = (byte)(year - person.BirthYear);

            
            double pDeath = (from x in DeathProbabilities
                             where x.Gender == person.Gender && x.kor == age
                             select x.halalvalsz).FirstOrDefault();
            
            if (rng.NextDouble() <= pDeath)
                person.IsAlive = false;

            
            if (person.IsAlive && person.Gender == Gender.Female)
            {
               
                double pBirth = (double)(from x in BirthProbabilities
                                 where x.Kor == age
                                 select x.valsz).FirstOrDefault();
                
                if (rng.NextDouble() <= pBirth)
                {
                    Person újszülött = new Person();
                    újszülött.BirthYear = year;
                    újszülött.NbrOfChildren = 0;
                    újszülött.Gender = (Gender)(rng.Next(1, 3));
                    Population.Add(újszülött);
                }
            }
            for (int i = 2005; i < numericUpDown1.Value; i++)
            {
                
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void start_button_Click(object sender, EventArgs e)
        {
            Simulation();
            
        }

        private void browes_btn_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog()==DialogResult.OK)
            {
                textBox1.Text = @"C:\Temp\nép.csv";
            }
        }
        void DisplayResults()
        {
            
        }
    }
}
