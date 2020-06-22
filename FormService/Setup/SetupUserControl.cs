using Kofax.Capture.AdminModule.InteropServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FormService
{
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface ISetupForm
    {
        [DispId(1)]
        AdminApplication Application { set; }
        [DispId(2)]
        void ActionEvent(int EventNumber, object Argument, out int Cancel);
    }

    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("FormService.Setup")]
    public class SetupUserControl : UserControl, ISetupForm
    {
        private AdminApplication adminApplication;
        public AdminApplication Application
        {
            
            set
            {
                value.AddMenu("FormService.Setup", "DateTimeFormatting - BatchField Setup", "BatchClass");
                adminApplication = value;
            }
        }

        public void ActionEvent(int EventNumber, object Argument, out int Cancel)
        {
            Cancel = 0;

            if ((KfxOcxEvent)EventNumber == KfxOcxEvent.KfxOcxEventMenuClicked && (string)Argument == "FormService.Setup")
            {
                SetupForm form = new SetupForm();
                form.ShowDialog(adminApplication.ActiveBatchClass);
            }
        }

    }
}
