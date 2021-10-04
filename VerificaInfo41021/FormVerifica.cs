using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VerificaInfo41021
{
    public partial class FormVerifica : Form
    {
        internal static List<Veicolo> Veicoli = new List<Veicolo>();
        internal static BindingSource bsVeicoli = new BindingSource();

        internal static Random r = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[r.Next(s.Length)]).ToArray());
        }

        public FormVerifica()
        {
            InitializeComponent();
        }

        private void FormVerifica_Load(object sender, EventArgs e)
        {
            Methods.LoadData();

            bsVeicoli.DataSource = Veicoli;
            dgvView.DataSource = bsVeicoli;

            cbBxEditSelect.DataSource = bsVeicoli;
            cbBxEditSelect.ValueMember = "Nome";
            cbBxEditSelect.DisplayMember = "Nome";

            cbBxEditSelect.SelectedIndex = -1;
            pnlEdit.Visible = false;

            lblAvgInc.Text = "€" + Methods.CalcAverageIncentive().ToString();
        }


        private void rdBtnAddEngine_CheckedChanged(object sender, EventArgs e)
        {
            if (rdBtnAddNoEngine.Checked)
            {
                rdBtnAddNoEngine.Checked = false;
                rdBtnAddEngine.Checked = true;

                nUpDwnAddPower.Enabled = true;
                nUpDwnAddTires.Enabled = false;
            }
        }

        private void rdBtnAddNoEngine_CheckedChanged(object sender, EventArgs e)
        {
            if (rdBtnAddEngine.Checked)
            {
                rdBtnAddEngine.Checked = false;
                rdBtnAddNoEngine.Checked = true;

                nUpDwnAddPower.Enabled = false;
                nUpDwnAddTires.Enabled = true;
            }
        }

        private void btnAddConferma_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(txBxAddName.Text) && !string.IsNullOrWhiteSpace(txBxAddDesc.Text) && nUpDwnAddPrice.Value > 0)
                {
                    if (rdBtnAddEngine.Checked)
                    {
                        if (nUpDwnAddPower.Value > 0)
                        {
                            Veicolo v = new VeicoloAMotore(txBxAddName.Text, txBxAddDesc.Text, (double)nUpDwnAddPrice.Value, (double)nUpDwnAddPower.Value);
                            v.CalcolaIncentivo(true);
                            Veicoli.Add(v);

                            bsVeicoli.DataSource = null;
                            bsVeicoli.DataSource = Veicoli;
                            dgvView.DataSource = null;
                            dgvView.DataSource = bsVeicoli;
                        }
                        else
                        {
                            throw new Exception("Verificare che i campi siano riempiti e maggiori di 0.");
                        }
                    }
                    else
                    {
                        if (nUpDwnAddTires.Value > 0)
                        {
                            Veicolo v = new VeicoloSenzaMotore(txBxAddName.Text, txBxAddDesc.Text, (double)nUpDwnAddPrice.Value, (int)nUpDwnAddTires.Value);
                            v.CalcolaIncentivo(true);
                            Veicoli.Add(v);

                            bsVeicoli.DataSource = null;
                            bsVeicoli.DataSource = Veicoli;
                            dgvView.DataSource = null;
                            dgvView.DataSource = bsVeicoli;
                        }
                        else
                        {
                            throw new Exception("Verificare che i campi siano riempiti e maggiori di 0.");
                        }
                    }

                    lblAvgInc.Text = "€" + Methods.CalcAverageIncentive().ToString();


                    txBxAddName.Clear();
                    txBxAddDesc.Clear();
                    nUpDwnAddPrice.Value = 0;
                    nUpDwnAddTires.Value = 0;
                    nUpDwnAddPower.Value = 0;

                    Methods.SaveData();
                }
                else
                {
                    throw new Exception("Verificare che i campi siano riempiti e maggiori di 0.");
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "Oops!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void rdBtnViewDefault_CheckedChanged(object sender, EventArgs e)
        {
            if (rdBtnViewRange.Checked)
            {
                rdBtnViewRange.Checked = false;
                rdBtnViewDefault.Checked = true;

                nUpDownViewRangeStart.Enabled = false;
                nUpDownViewRangeEnd.Enabled = false;
                btnViewCalc.Enabled = false;

                dgvView.DataSource = bsVeicoli;
            }
        }

        private void rdBtnViewRange_CheckedChanged(object sender, EventArgs e)
        {
            if (rdBtnViewDefault.Checked)
            {
                rdBtnViewDefault.Checked = false;
                rdBtnViewRange.Checked = true;

                nUpDownViewRangeStart.Enabled = true;
                nUpDownViewRangeEnd.Enabled = true;
                btnViewCalc.Enabled = true;
            }
        }

        private void btnViewCalc_Click(object sender, EventArgs e)
        {
            List<Veicolo> inrange = new List<Veicolo>();

            foreach (Veicolo v in Veicoli)
            {
                if (v.Prezzo >= (double)nUpDownViewRangeStart.Value && v.Prezzo <= (double)nUpDownViewRangeEnd.Value)
                {
                    inrange.Add(v);
                }
            }

            BindingSource bsRange = new BindingSource();
            bsRange.DataSource = inrange;
            dgvView.DataSource = bsRange;
        }

        private void cbBxEditSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbBxEditSelect.SelectedItem != null)
            {
                var mod = cbBxEditSelect.SelectedItem;

                if (mod.GetType() == typeof(VeicoloAMotore))
                {
                    VeicoloAMotore vmod = (VeicoloAMotore)cbBxEditSelect.SelectedItem;

                    txBxEditName.Text = vmod.Nome;
                    txBxEditDesc.Text = vmod.Descrizione;

                    nUpDwnEditPrice.Value = (decimal)vmod.Prezzo;
                    nUpDwnEditPower.Value = (decimal)vmod.Potenza;

                    rdBtnEditEngine.Checked = true;
                    nUpDwnEditPower.Enabled = true;

                    rdBtnEditNoEngine.Checked = false;
                    nUpDwnEditTires.Enabled = false;
                }
                else
                {
                    VeicoloSenzaMotore vmod = (VeicoloSenzaMotore)cbBxEditSelect.SelectedItem;

                    txBxEditName.Text = vmod.Nome;
                    txBxEditDesc.Text = vmod.Descrizione;

                    nUpDwnEditPrice.Value = (decimal)vmod.Prezzo;
                    nUpDwnEditTires.Value = vmod.NRuote;

                    rdBtnEditNoEngine.Checked = true;
                    nUpDwnEditTires.Enabled = true;

                    rdBtnEditEngine.Checked = false;
                    nUpDwnEditPower.Enabled = false;
                }

                pnlEdit.Visible = true;

            }
        }

        private void btnDeleteVehicle_Click(object sender, EventArgs e)
        {
            if (cbBxEditSelect.SelectedItem != null)
            {
                DialogResult dr = MessageBox.Show("Sei sicuro di voler eliminare questo veicolo?", "Attenzione!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if(dr == DialogResult.Yes)
                {
                    Veicoli.Remove((Veicolo)cbBxEditSelect.SelectedItem);
                    pnlEdit.Visible = false;

                    bsVeicoli.DataSource = null;
                    bsVeicoli.DataSource = Veicoli;
                    dgvView.DataSource = null;
                    dgvView.DataSource = bsVeicoli;

                    cbBxEditSelect.DataSource = null;
                    cbBxEditSelect.DataSource = bsVeicoli;
                    cbBxEditSelect.ValueMember = "Nome";
                    cbBxEditSelect.DisplayMember = "Nome";

                    lblAvgInc.Text = "€" + Methods.CalcAverageIncentive().ToString();

                    Methods.SaveData();
                }
            }
        }

        private void btnEditConfirm_Click(object sender, EventArgs e)
        {
            if (cbBxEditSelect.SelectedItem != null)
            {
                var mod = cbBxEditSelect.SelectedItem;

                if (mod.GetType() == typeof(VeicoloAMotore))
                {
                    VeicoloAMotore vmod = (VeicoloAMotore)cbBxEditSelect.SelectedItem;

                    vmod.Nome = txBxEditName.Text;

                    vmod.Descrizione = txBxEditDesc.Text;

                    if(vmod.Prezzo != (double)nUpDwnEditPrice.Value)
                    {
                        vmod.Prezzo = (double)nUpDwnEditPrice.Value;
                        vmod.CalcolaIncentivo(true);
                    }
                    
                    vmod.Potenza = (double)nUpDwnEditPower.Value;
                }
                else
                {
                    VeicoloSenzaMotore vmod = (VeicoloSenzaMotore)cbBxEditSelect.SelectedItem;

                    vmod.Nome = txBxEditName.Text;

                    vmod.Descrizione = txBxEditDesc.Text;

                    if (vmod.Prezzo != (double)nUpDwnEditPrice.Value)
                    {
                        vmod.Prezzo = (double)nUpDwnEditPrice.Value;
                        vmod.CalcolaIncentivo(true);
                    }

                    vmod.NRuote = (int)nUpDwnEditTires.Value;
                }

                bsVeicoli.DataSource = null;
                bsVeicoli.DataSource = Veicoli;
                dgvView.DataSource = null;
                dgvView.DataSource = bsVeicoli;

                cbBxEditSelect.DataSource = null;
                cbBxEditSelect.DataSource = bsVeicoli;
                cbBxEditSelect.ValueMember = "Nome";
                cbBxEditSelect.DisplayMember = "Nome";

                lblAvgInc.Text = "€" + Methods.CalcAverageIncentive().ToString();

                pnlEdit.Visible = false;
                cbBxEditSelect.SelectedIndex = -1;

                Methods.SaveData();
            }
        }
    }
}
