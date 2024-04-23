using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace presentacion
{
    /// <summary>
    /// Lógica de interacción para PrintWindow.xaml
    /// </summary>
    public partial class PrintWindow : Window
    {
        private FixedDocumentSequence _document;
        public PrintWindow(FixedDocumentSequence document)
        {
            _document = document;
            InitializeComponent();
            PreviewD.Document = document;// as IDocumentPaginatorSource;
            // Eliminamos la ventana de búsqueda
            DocumentViewer dv1 = LogicalTreeHelper.FindLogicalNode(this, "PreviewD") as DocumentViewer;
            ContentControl cc = dv1.Template.FindName("PART_FindToolBarHost", dv1) as ContentControl;
            cc.Visibility = Visibility.Collapsed;
        }
        public PrintWindow()
        {
            //_document = document;
            InitializeComponent();
            //PreviewD.Document = document;// as IDocumentPaginatorSource;
            // Eliminamos la ventana de búsqueda
            //DocumentViewer dv1 = LogicalTreeHelper.FindLogicalNode(this, "PreviewD") as DocumentViewer;
            //ContentControl cc = dv1.Template.FindName("PART_FindToolBarHost", dv1) as ContentControl;
            //cc.Visibility = Visibility.Collapsed;
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _document = null;
        }
    }
}
