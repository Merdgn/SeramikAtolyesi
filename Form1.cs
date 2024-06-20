using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeramikAtolyesi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        NpgsqlConnection baglanti = new NpgsqlConnection("server=localHost; port=5432;Database=SeramikAtolyesi; user ID=postgres; password=1100");

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void BtnListele_Click(object sender, EventArgs e)
        {
            try
            {
                string sorgu = "SELECT * FROM \"SeramikAtolyesi\".\"Urun\"";
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu, baglanti);
                DataSet ds = new DataSet();
                da.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];

                MessageBox.Show("Listeleme başarıyla gerçekleşti.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Listeleme hatası: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void BtnEkle_Click(object sender, EventArgs e)
        {
            try
            {
                // TxtUrunID.Text'in boş olup olmadığını kontrol et
                if (string.IsNullOrEmpty(TxtUrunID.Text))
                {
                    MessageBox.Show("UrunID alanı boş bırakılamaz.");
                    return;
                }

                baglanti.Open();

                NpgsqlCommand komut = new NpgsqlCommand("INSERT INTO \"SeramikAtolyesi\".\"Urun\"(\"UrunID\", \"UrunAdi\", \"BirimFiyat\") VALUES (@p1, @p2, @p3)", baglanti);

                // Kullanıcının girdiği UrunID değerini kullan
                komut.Parameters.AddWithValue("@p1", int.Parse(TxtUrunID.Text));
                komut.Parameters.AddWithValue("@p2", TxtUrunAdi.Text);
                komut.Parameters.AddWithValue("@p3", decimal.Parse(TxtBirimFiyat.Text));

                komut.ExecuteNonQuery();

                MessageBox.Show("Ürün ekleme işlemi başarılı bir şekilde gerçekleşti");
                BtnListele_Click(sender, e); // Ekleme işleminden sonra listeyi güncelle
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ekleme hatası: " + ex.Message);
            }
            finally
            {
                baglanti.Close();
            }
        }


        private void BtnSil_Click(object sender, EventArgs e)
{
    try
    {
        baglanti.Open();

        // Önce SiparisUrun tablosundan ilgili kayıtları sil
        NpgsqlCommand siparisUrunSilKomut = new NpgsqlCommand("DELETE FROM \"SeramikAtolyesi\".\"SiparisUrun\" WHERE \"UrunKodu\" = @p1", baglanti);
        siparisUrunSilKomut.Parameters.AddWithValue("@p1", int.Parse(TxtUrunID.Text));
        siparisUrunSilKomut.ExecuteNonQuery();

        // Ardından Stok tablosundan ilgili kayıtları sil
        NpgsqlCommand stokSilKomut = new NpgsqlCommand("DELETE FROM \"SeramikAtolyesi\".\"Stok\" WHERE \"UrunID\" = @p1", baglanti);
        stokSilKomut.Parameters.AddWithValue("@p1", int.Parse(TxtUrunID.Text));
        stokSilKomut.ExecuteNonQuery();

        // Son olarak Urun tablosundan ilgili kaydı sil
        NpgsqlCommand urunSilKomut = new NpgsqlCommand("DELETE FROM \"SeramikAtolyesi\".\"Urun\" WHERE \"UrunID\" = @p1", baglanti);
        urunSilKomut.Parameters.AddWithValue("@p1", int.Parse(TxtUrunID.Text));
        urunSilKomut.ExecuteNonQuery();

        MessageBox.Show("Ürün silme işlemi başarılı bir şekilde gerçekleşti");
        BtnListele_Click(sender, e); // Silme işleminden sonra listeyi güncelle
    }
    catch (Exception ex)
    {
        MessageBox.Show("Silme hatası: " + ex.Message);
    }
    finally
    {
        baglanti.Close();
    }
}


        private void BtnGuncelle_Click(object sender, EventArgs e)
        {
            try
            {
                baglanti.Open();

                NpgsqlCommand komut = new NpgsqlCommand("UPDATE \"SeramikAtolyesi\".\"Urun\" SET \"UrunAdi\" = @p1, \"BirimFiyat\" = @p2 WHERE \"UrunID\" = @p3", baglanti);

                komut.Parameters.AddWithValue("@p1", TxtUrunAdi.Text);
                komut.Parameters.AddWithValue("@p2", decimal.Parse(TxtBirimFiyat.Text));
                komut.Parameters.AddWithValue("@p3", int.Parse(TxtUrunID.Text));

                komut.ExecuteNonQuery();

                MessageBox.Show("Ürün güncelleme işlemi başarılı bir şekilde gerçekleşti");
                BtnListele_Click(sender, e); // Güncelleme işleminden sonra listeyi güncelle
            }
            catch (Exception ex)
            {
                MessageBox.Show("Güncelleme hatası: " + ex.Message);
            }
            finally
            {
                baglanti.Close();
            }
        }
    }
}

