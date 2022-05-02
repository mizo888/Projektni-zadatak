

using System.Data;
using System.Data.SqlClient;


namespace Upravljanje_Zaposlenicima
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //CONNECTION TO DATABASE
        SqlConnection con = new SqlConnection("Data Source=DESKTOP-M3ELO4D;Initial Catalog=MyDBTest;Integrated Security=True");

        //CREATE
        private void button1_Click(object sender, EventArgs e)
        {

            //con.Open();
            //SqlCommand command = new SqlCommand("insert into Zaposlenici values ('" + int.Parse(textBox1.Text) + "','" + textBox2.Text + "','" + textBox3.Text + "','" + int.Parse(comboBox1.Text) + "','" + textBox5.Text + "','" + textBox6.Text + "',getdate(),NULL)", con);
            //command.ExecuteNonQuery();
            //MessageBox.Show("Uspjesno unesen podatak.");
            //con.Close();
            if (textBox1_Required.Text == "" || textBox2_Required.Text == "" || textBox3_Required.Text == "" || comboBox1_Required.Text=="" || textBox5_Required.Text=="" || textBox6_Required.Text=="")
            {
                MessageBox.Show("Sva polja moraju biti popunjena", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                foreach (Control ctl in panel1.Controls)
                {
                    if (ctl is TextBox || ctl is ComboBox)
                    {
                        int find = ctl.Name.ToString().IndexOf("_Required", 0);
                        if (find > 0 && ctl.Text == "")
                        {
                            ctl.BackColor = Color.Red;
                            lblErrorMessage.Text = "Sva polja su mandatorna.";
                            lblErrorMessage.ForeColor = Color.Red;
                            lblErrorMessage.Visible = true;

                        }                      
                    }
                }
            }
            else if (!int.TryParse(textBox1_Required.Text, out _))
            {
                MessageBox.Show("U polje 'Sifra' moguce je unijeti samo brojeve u formatu '001','002'", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if(textBox1_Required.Text.Length > 3)
            {
                MessageBox.Show("Maksimalno tri broja u polje 'Sifra'", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            else
            {
                //CHECK FOR DUPLICATE PASSWORD
                con.Open();
                bool readerHasRows = false; // <-- Initialize bool here for later use

                string sifra = textBox1_Required.Text;
                

                
                string commandQuery = "SELECT Sifra FROM Zaposlenici WHERE Sifra = @Sifra";
                using (SqlCommand cmd = new SqlCommand(commandQuery, con))
                {
                    cmd.Parameters.AddWithValue("@Sifra", textBox1_Required.Text);
                    


                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // bool initialized above is set here
                        readerHasRows = (reader != null && reader.HasRows);
                        con.Close();
                    }
                }

                if (readerHasRows)
                {
                    //MessageBox.Show("Sifra vec postoji, pokusajte neku drugu");
                    MessageBox.Show("Sifra vec postoji, pokusajte neku drugu", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                else
                {
                    con.Open();
                    SqlCommand command = new SqlCommand("INSERT INTO Zaposlenici(Sifra,Prezime,Ime,Pol,Grad,Adresa,DatumDodavanja) VALUES (@Sifra,@Prezime,@Ime,@Pol,@Grad,@Adresa,getdate())", con);
                    command.Parameters.AddWithValue("@Sifra", textBox1_Required.Text);
                    command.Parameters.AddWithValue("@Prezime", textBox2_Required.Text);
                    command.Parameters.AddWithValue("@Ime", textBox3_Required.Text);
                    command.Parameters.AddWithValue("@Pol", int.Parse(comboBox1_Required.Text));
                    command.Parameters.AddWithValue("@Grad", textBox5_Required.Text);
                    command.Parameters.AddWithValue("@Adresa", textBox6_Required.Text);
                    command.ExecuteNonQuery();
                    con.Close();

                    textBox1_Required.Text = "";
                    textBox2_Required.Text = "";
                    textBox3_Required.Text = "";
                    comboBox1_Required.Text = "";
                    textBox5_Required.Text = "";
                    textBox6_Required.Text = "";
                    MessageBox.Show("Uspjesno unesen podatak.");
                    ShowData();
                }
            }

        }



        //SHOW DATA
        void ShowData()
        {
            SqlCommand command = new SqlCommand("select * from Zaposlenici", con);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            dataGridView1.DataSource = dt;

        }
        //SHOW TABLE DATA ON FORM LOAD
        private void Form1_Load(object sender, EventArgs e)
        {
            ShowData();
        }




        //UPDATE

        private void button2_Click(object sender, EventArgs e)
        {

            if (textBox1_Required.Text == "" || textBox2_Required.Text == "" || textBox3_Required.Text == "" || comboBox1_Required.Text == "" || textBox5_Required.Text == "" || textBox6_Required.Text == "")
            {
                MessageBox.Show("Sva polja moraju biti popunjena", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                foreach (Control ctl in panel1.Controls)
                {
                    if (ctl is TextBox || ctl is ComboBox)
                    {
                        int find = ctl.Name.ToString().IndexOf("_Required", 0);
                        if (find > 0 && ctl.Text == "")
                        {
                            ctl.BackColor = Color.Red;
                            lblErrorMessage.Text = "Sva polja su mandatorna.";
                            lblErrorMessage.ForeColor = Color.Red;
                            lblErrorMessage.Visible = true;

                        }
                    }
                }
            }
            else if (!int.TryParse(textBox1_Required.Text, out _) || textBox1_Required.Text =="")
            {
                MessageBox.Show("U polje 'Sifra' moguce je unijeti samo brojeve u formatu '001','002'", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (textBox1_Required.Text.Length > 3)
            {
                MessageBox.Show("Maksimalno tri broja", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            else
            {
                if (!int.TryParse(textBox4.Text, out _) || textBox4.Text == "")
                {

                    MessageBox.Show("Upisite Id koji zelite izmijeniti", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);                
                }
                else
                {


                    //CHEK FOR DUPLICATE PASSWORDS
                    con.Open();
                    bool readerHasRows = false; // <-- Initialize bool here for later use

                    string sifra = textBox1_Required.Text;


                    
                    string commandQuery = "SELECT Sifra FROM Zaposlenici WHERE Sifra = @Sifra";
                    using (SqlCommand cmd = new SqlCommand(commandQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@Sifra", textBox1_Required.Text);



                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            // bool initialized above is set here
                            readerHasRows = (reader != null && reader.HasRows);
                            con.Close();
                        }
                    }

                    if (readerHasRows)
                    {
                        //MessageBox.Show("Sifra vec postoji, pokusajte neku drugu");
                        MessageBox.Show("Sifra vec postoji, pokusajte neku drugu", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        //CHECK TO SEE IF Id EXISTS
                        con.Open();
                        bool idHasRows =false; // <-- Initialize bool here for later use

                        string idHas = textBox4.Text;

                        string commandId = "SELECT Id FROM Zaposlenici WHERE Id = @Id";
                        using (SqlCommand cmd = new SqlCommand(commandId, con))
                        {
                            cmd.Parameters.AddWithValue("@Id", textBox4.Text);



                            using (SqlDataReader id = cmd.ExecuteReader())
                            {
                                // bool initialized above is set here
                                idHasRows = !(id.HasRows);
                                con.Close();
                            }
                        }
                        if (idHasRows)
                        {
                            //MessageBox.Show("Id ne postoji");
                            MessageBox.Show("Id ne postoji", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            if (comboBox1_Required.Text == "" || !int.TryParse(comboBox1_Required.Text, out _))
                            {
                                MessageBox.Show("Izaberite jednu od ponudjenih opcija. '1' ili '2' u polje 'Pol'.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                con.Open();
                                SqlCommand command = new SqlCommand("update Zaposlenici set Sifra = '" + textBox1_Required.Text + "',Prezime = '" + textBox2_Required.Text + "',Ime = '" + textBox3_Required.Text + "',Pol ='" + int.Parse(comboBox1_Required.Text) + "',Grad ='" + textBox5_Required.Text + "',Adresa ='" + textBox6_Required.Text + "',DatumIzmjene = getdate() where Id = '" + int.Parse(textBox4.Text) + "'", con);
                                command.ExecuteNonQuery();
                                con.Close();
                                MessageBox.Show("Uspjesno izmijenjen podatak.");
                                ShowData();
                            }   

                        }

                    }
 
                }
            }
        }




        //DELETE
        private void button3_Click(object sender, EventArgs e)
        {
            //CANNOT BE EMPTY AND MUST BY OF TYPE INT
            if(textBox1.Text == "" || !int.TryParse(textBox1.Text, out _))
            {
                MessageBox.Show("Upisite Id koji zelite izbrisati", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {

                //CHECK TO SEE IF Id EXISTS
                con.Open();
                bool idHasRows = false; // <-- Initialize bool here for later use

                string idHas = textBox1.Text;

                string commandId = "SELECT Id FROM Zaposlenici WHERE Id = @Id";
                using (SqlCommand cmd = new SqlCommand(commandId, con))
                {
                    cmd.Parameters.AddWithValue("@Id", textBox1.Text);



                    using (SqlDataReader id = cmd.ExecuteReader())
                    {
                        // bool initialized above is set here
                        idHasRows = !(id.HasRows);
                        con.Close();
                    }
                }
                if (idHasRows)
                {
                    //MessageBox.Show("Id ne postoji");
                    MessageBox.Show("Id ne postoji", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    DialogResult result = MessageBox.Show("Da li sigurni?","Podatak ce biti izbrisan!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if(result == DialogResult.Yes)
                    {
                    con.Open();
                    SqlCommand command = new SqlCommand("DELETE Zaposlenici WHERE Id='" + int.Parse(textBox1.Text) + "'", con);
                    //command.Parameters.AddWithValue("@Id", int.Parse(textBox1.Text));
                    command.ExecuteNonQuery();
                    con.Close();

                    textBox1.Text = "";
                    MessageBox.Show("Uspjesno izbrisan podatak.");
                    ShowData();
                    }
                }
                
            }
        }





        //SEARCH
        private void button4_Click(object sender, EventArgs e)
        {
            //CANNOT BE EMPTY AND MUST BY OF TYPE INT
            if (textBox2.Text == "" || !int.TryParse(textBox2.Text, out _))
            {
                MessageBox.Show("Upisite Id koji zelite pretraziti", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {

                //CHECK TO SEE IF Id EXISTS
                con.Open();
                bool idHasRows = false; // <-- Initialize bool here for later use

                string idHas = textBox2.Text;

                string commandId = "SELECT Id FROM Zaposlenici WHERE Id = @Id";
                using (SqlCommand cmd = new SqlCommand(commandId, con))
                {
                    cmd.Parameters.AddWithValue("@Id", textBox2.Text);



                    using (SqlDataReader id = cmd.ExecuteReader())
                    {
                        // bool initialized above is set here
                        idHasRows = !(id.HasRows);
                        con.Close();
                    }
                }
                if (idHasRows)
                {
                    //MessageBox.Show("Id ne postoji");
                    MessageBox.Show("Id ne postoji", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    con.Open();
                    SqlCommand command = new SqlCommand("select * from Zaposlenici WHERE id = @Id", con);
                    command.Parameters.AddWithValue("@Id", textBox2.Text);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridView1.DataSource = dt;
                    //DataSet ds = new DataSet();
                    
                    if(dt.Rows.Count > 0)
                    {
                        textBox1_Required.Text = dt.Rows[0]["Sifra"].ToString();
                        textBox2_Required.Text = dt.Rows[0]["Prezime"].ToString();
                        textBox3_Required.Text = dt.Rows[0]["Ime"].ToString();
                        comboBox1_Required.Text = dt.Rows[0]["Pol"].ToString();
                        textBox5_Required.Text = dt.Rows[0]["Grad"].ToString();
                        textBox6_Required.Text = dt.Rows[0]["Adresa"].ToString();
                    }
                    con.Close();
                }
            }
        }
    }
}
