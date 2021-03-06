﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Cloo;
using OpenCLTemplate;


namespace GpuCalc
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            float[] x = new float[] {0};
            float[] y = new float[] {0};
            CLCalc.InitCL();
            if (!string.IsNullOrWhiteSpace(textBox1.Text) && !string.IsNullOrWhiteSpace(textBox2.Text))
            {
               x[0] = (float)Convert.ToDouble(textBox1.Text.Replace('.', ','));
               y[0] = (float)Convert.ToDouble(textBox2.Text.Replace('.', ','));
            }

            string s = @"
            __kernel void
            sum(global float4 *x, global float4 *y) {
                x[0] = x[0] + y[0];
            }";

            CLCalc.Program.Compile(new string[] { s });
            CLCalc.Program.Kernel sum = new CLCalc.Program.Kernel("sum");

            CLCalc.Program.Variable varx = new CLCalc.Program.Variable(x);
            CLCalc.Program.Variable vary = new CLCalc.Program.Variable(y);

            CLCalc.Program.Variable[] args = { varx, vary };
            int[] max = new int[] { 1 };

            sum.Execute(args, max);
            varx.ReadFromDeviceTo(x);
            textBox3.Text = Convert.ToString(x[0]);

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsControl(e.KeyChar)) return;
            if (Char.IsDigit(e.KeyChar)) return;
            if (e.KeyChar == ',') return;
            e.Handled = true;
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsControl(e.KeyChar)) return;
            if (Char.IsDigit(e.KeyChar)) return;
            if (e.KeyChar == ',') return;
            e.Handled = true;
        }
    }
}
