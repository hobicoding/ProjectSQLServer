using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ProjectSQLServer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            FillDataToList();
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            if(tbNama.Text=="")
            {
                MessageBox.Show("Nama tidak boleh kosong");
            }
            else if(cbJenisKelamin.Text=="")
            {
                MessageBox.Show("Jenis Kelamin tidak boleh kosong");
            }
            else
            {
                try
                {
                    Data.Connect();
                    Data.BeginTrans();

                    Data.Command("INSERT INTO Biodata(Nama,JenisKelamin) VALUES(@0,@1)", 
                        new object[] { tbNama.Text, cbJenisKelamin.Text });
                    Data.CommitTrans();
                    MessageBox.Show("Data berhasil disimpan!");
                    tbNama.Text = "";
                    cbJenisKelamin.Text = "";
                }
                catch (Exception ex)
                {
                    Data.RollbackTrans();
                    MessageBox.Show(ex.ToString(),"Data gagal disimpan!");
                }
                finally
                {
                    Data.Disconnect();
                }
            }

            FillDataToList();
        }

        private void FillDataToList()
        {
            try
            {
                Data.Connect();

                DataTable dt = Data.SelectDatatable("Select * FROM Biodata", new object[] { });

                listView1.Items.Clear();
                listView1.Columns.Clear();
                listView1.Columns.Add("Nomor", 60, HorizontalAlignment.Left);
                listView1.Columns.Add("Nama", 100, HorizontalAlignment.Left);
                listView1.Columns.Add("JenisKelamin", 100, HorizontalAlignment.Left);

                listView1.GridLines = true;
                listView1.FullRowSelect = true;
                listView1.MultiSelect = false;
                listView1.View = View.Details;

                if(dt!=null)
                {
                    if(dt.Rows.Count>0)
                    {
                        foreach(DataRow row in dt.Rows)
                        {
                            ListViewItem item = new ListViewItem(row[0].ToString());
                            for(int i=1;i<dt.Columns.Count;i++)
                            {
                                item.SubItems.Add(row[i].ToString());
                            }
                            listView1.Items.Add(item);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "GAGAL");
            }
            finally
            {
                Data.Disconnect();
            }
        }

        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if(e.IsSelected)
            {
                try
                {
                    tbNomor.Text = listView1.SelectedItems[0].Text;
                    tbNama.Text = listView1.SelectedItems[0].SubItems[1].Text;
                    cbJenisKelamin.Text = listView1.SelectedItems[0].SubItems[2].Text;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            if(tbNama.Text=="")
            {
                MessageBox.Show("Pilih data yang ingin dihapus terlebih dahulu");
            }
            else
            {
                try
                {
                    Data.Connect();
                    Data.BeginTrans();
                    Data.Command("DELETE FROM Biodata WHERE Nomor=@0", new object[] { Convert.ToInt32(tbNomor.Text) });
                    Data.CommitTrans();
                    MessageBox.Show("Berhasil menghapus data!");
                    tbNomor.Text = "";
                    tbNama.Text = "";
                    cbJenisKelamin.Text = "";
                }
                catch(Exception ex)
                {
                    Data.RollbackTrans();
                    MessageBox.Show(ex.ToString(),"Gagal Menghapus Data");
                }
                finally
                {
                    Data.Disconnect();
                }
            }

            FillDataToList();
        }
    }
}
