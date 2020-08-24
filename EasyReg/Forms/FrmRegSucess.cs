using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EasyReg.Class;

namespace EasyReg.Forms
{
    public partial class FrmRegSucess : Form
    {
        RegData _reg;
        public FrmRegSucess()
        {
            InitializeComponent();
        }

        public FrmRegSucess(RegData reg)
        {
            InitializeComponent();
            _reg = reg;
            lblAddress.Text = String.Format("{0},{1} {2} {3}", _reg.House, _reg.Street, _reg.Town, _reg.State);
            lblCap.Text = _reg.Capacity;
            lblChasis.Text = _reg.Chasis;
            lblColor.Text = _reg.Color;
            lblEngine.Text = _reg.Engine;
            lblMake.Text = _reg.Make;
            lblName.Text = String.Format("{0} {1}", _reg.Surname, _reg.OtherName);
            lblRegDate.Text = _reg.Date;
            lblRegPlateNum.Text = _reg.Plate;
        }

    }
}
