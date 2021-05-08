using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Net.Http;
using System.IO;

namespace HTTPCats
{
    public partial class Form1 : Form
    {
        private List<int> codes = new List<int>
        {
            100, 101, 102, 200, 201, 202, 204, 206, 207, 300, 301, 302, 303, 304, 305, 307, 308, 400, 401, 402, 403, 404, 405, 406, 408, 409, 410, 411, 412, 413, 414, 415, 416, 417, 418, 420, 421, 422, 423, 424, 425, 426, 429, 431, 444, 450, 451, 499, 500, 504, 506, 507, 508, 509, 510, 511, 599
        };
        public static HttpClient client = new HttpClient();
        public Form1()
        {
            InitializeComponent();
            foreach (int i in codes)
                comboBox1.Items.Add(i);
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string urlstring = "https://1dxdf6xgll.execute-api.us-west-2.amazonaws.com/HTTPCode";
            urlstring += "?code=" + comboBox1.SelectedItem.ToString();
            byte[] bytes = Convert.FromBase64String(await client.GetStringAsync(urlstring));
            pictureBox1.Image = Image.FromStream(new MemoryStream(bytes));
        }
    }
}
