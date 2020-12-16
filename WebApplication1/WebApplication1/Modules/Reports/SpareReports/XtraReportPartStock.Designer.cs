namespace DXBMS.Modules.Reports.SpareReports
{
    partial class XtraReportPartStock
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.dataSetPartStockReport1 = new DXBMS.DataSetPartStockReport();
            //this.sp_PartsStockSourceWiseNewTableAdapter = new DXBMS.DataSetPartStockReportTableAdapters.sp_PartsStockSourceWiseNewTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetPartStockReport1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // TopMargin
            // 
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // dataSetPartStockReport1
            // 
            this.dataSetPartStockReport1.DataSetName = "DataSetPartStockReport";
            this.dataSetPartStockReport1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // sp_PartsStockSourceWiseNewTableAdapter
            // 
            //this.sp_PartsStockSourceWiseNewTableAdapter.ClearBeforeFill = true;
            // 
            // XtraReportPartStock
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin});
            //this.DataAdapter = this.sp_PartsStockSourceWiseNewTableAdapter;
            this.DataMember = "sp_PartsStockSourceWiseNew";
            this.DataSource = this.dataSetPartStockReport1;
            this.Version = "13.2";
            ((System.ComponentModel.ISupportInitialize)(this.dataSetPartStockReport1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DataSetPartStockReport dataSetPartStockReport1;
        //private DataSetPartStockReportTableAdapters.sp_PartsStockSourceWiseNewTableAdapter sp_PartsStockSourceWiseNewTableAdapter;
    }
}
