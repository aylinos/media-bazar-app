using MediaBazarProject.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaBazarProject
{
    public static class CustomMessageBoxController
    {
        public static System.Windows.Forms.DialogResult ShowMessage(string message, System.Windows.Forms.MessageBoxButtons button)
        {
            System.Windows.Forms.DialogResult dlgResult = System.Windows.Forms.DialogResult.None;
            switch (button)
            {
                case System.Windows.Forms.MessageBoxButtons.OK:
                    using (frmMessageOK msgOK = new frmMessageOK())
                    {
                        msgOK.Message = message;
                        dlgResult = msgOK.ShowDialog();
                    }
                    break;
                case System.Windows.Forms.MessageBoxButtons.YesNo:
                    using(frmMessageYesNo msgYesNo = new frmMessageYesNo())
                    {
                        msgYesNo.Message = message;
                        dlgResult = msgYesNo.ShowDialog();
                    }
                    break;
            }
            return dlgResult;
        }
    }
}
