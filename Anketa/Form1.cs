using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Anketa
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
         

        private void btn_add_change_Click(object sender, EventArgs e)
        {
            if (btn_add_change.Text == "Add")
            {
                if (string.IsNullOrEmpty(gTB_name.Text) || string.IsNullOrEmpty(gTB_surname.Text) ||
                string.IsNullOrEmpty(gTB_phone.Text) || string.IsNullOrEmpty(gTB_email.Text) ||
                dtP_birthday.Value == DateTime.Now) MessageBox.Show("Please fill in all the information");
                else
                {
                    if(!Regex.IsMatch(gTB_email.Text, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"))
                    {
                        MessageBox.Show("Valid Email");
                        gTB_email.Text = string.Empty;
                    }
                    else if(!Regex.IsMatch(gTB_phone.Text, @"^(?:\(?)(\d{3})(?:[\).\s]?)(\d{3})(?:[-\.\s]?)(\d{4})(?!\d)"))
                    {
                        MessageBox.Show("Valid Phone number");
                        gTB_phone.Text = string.Empty;
                    }
                    else
                    {
                        Person person = new Person(gTB_name.Text, gTB_surname.Text, gTB_email.Text, gTB_phone.Text, dtP_birthday.Value);
                        listBox.Items.Add(person);
                        gTB_name.Text = string.Empty;
                        gTB_surname.Text = string.Empty;
                        gTB_email.Text = string.Empty;
                        gTB_phone.Text = string.Empty;
                    }
                    
                }
            }
            else
            {
                var index = listBox.SelectedIndex;
                (listBox.SelectedItem as Person).Name = gTB_name.Text;
                (listBox.SelectedItem as Person).Email = gTB_email.Text;
                (listBox.SelectedItem as Person).Surname = gTB_surname.Text;
                (listBox.SelectedItem as Person).Phone = gTB_phone.Text;
                (listBox.SelectedItem as Person).BirthDate = dtP_birthday.Value;
                var person = (listBox.SelectedItem as Person);
                listBox.Items.RemoveAt(index);
                listBox.Items.Insert(index, person);
               
                btn_add_change.Text = "Add";
            }
            
        }

      

        private void listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gTB_name.Text = (listBox.SelectedItem as Person)?.Name;
                gTB_email.Text = (listBox.SelectedItem as Person)?.Email;
                gTB_surname.Text = (listBox.SelectedItem as Person)?.Surname;
                gTB_phone.Text = (listBox.SelectedItem as Person)?.Phone;
                dtP_birthday.Value = (listBox.SelectedItem as Person).BirthDate;
                btn_add_change.Text = "Change";
            }
            catch 
            {

                
            }
            
        }

        private void gTB_name_TextChanged(object sender, EventArgs e)
        {
            if (!Regex.IsMatch(gTB_name.Text, @"^[A-Za-z]+$")) gTB_name.Text = string.Empty;
        }

        private void gTB_surname_TextChanged(object sender, EventArgs e)
        {
            if (!Regex.IsMatch(gTB_surname.Text, @"^[A-Za-z]+$")) gTB_surname.Text= string.Empty;
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(gTB_filename.Text))
            {
                MessageBox.Show("File name is Empty");
            }
            else if (string.IsNullOrEmpty(gTB_name.Text) || string.IsNullOrEmpty(gTB_surname.Text) ||
               string.IsNullOrEmpty(gTB_phone.Text) || string.IsNullOrEmpty(gTB_email.Text) ||
               dtP_birthday.Value == DateTime.Now) MessageBox.Show("Please fill in all the information");
            else
            {
                if (!Regex.IsMatch(gTB_email.Text, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"))
                {
                    MessageBox.Show("Valid Email");
                    gTB_email.Text = string.Empty;
                }
                else if (!Regex.IsMatch(gTB_phone.Text, @"^(?:\(?)(\d{3})(?:[\).\s]?)(\d{3})(?:[-\.\s]?)(\d{4})(?!\d)"))
                {
                    MessageBox.Show("Valid Phone number");
                    gTB_phone.Text = string.Empty;
                }
                else
                {
                    Person person = new Person(gTB_name.Text, gTB_surname.Text, gTB_email.Text, gTB_phone.Text, dtP_birthday.Value);
                    var str = JsonConvert.SerializeObject(person, Newtonsoft.Json.Formatting.Indented);
                    File.WriteAllText($"{gTB_filename.Text}.json",str);
                    MessageBox.Show("Save successfuly!");
                    gTB_filename.Text = string.Empty;
                }
                
            }
        }

        private void btn_load_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(gTB_filename.Text))
            {
                MessageBox.Show("File name is Empty");
            }
            else if (File.Exists($"{gTB_filename.Text}.json"))
            {
                var str = File.ReadAllText($"{gTB_filename.Text}.json");
                var person = JsonConvert.DeserializeObject<Person>(str);
                gTB_name.Text = person.Name;
                gTB_email.Text = person.Email;
                gTB_surname.Text = person.Surname;
                gTB_phone.Text = person.Phone;
                gTB_filename.Text = string.Empty;
            }
            else
            {
                MessageBox.Show("File not available");
                gTB_filename.Text = string.Empty;
            }

        }
    }
}
