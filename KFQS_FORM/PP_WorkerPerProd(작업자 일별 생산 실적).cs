using DC_POPUP;
using DC00_assm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace KFQS_Form
{
    public partial class PP_WorkerPerProd : DC00_WinForm.BaseMDIChildForm
    {
        // 그리드를 세팅 할 수 있드록 도와주는 함수 클래스
        UltraGridUtil _GridUtil = new UltraGridUtil();
        // 공장 변수 입력
        private string sPlantCode = LoginInfo.UserID;

        public PP_WorkerPerProd()
        {
            InitializeComponent();
        }

        private void PP_WorkerPerProd_Load(object sender, EventArgs e)
        {
            // 그리드 세팅
            try
            {

                _GridUtil.InitializeGrid(this.grid1, false, true, false, "", false);
                _GridUtil.InitColumnUltraGrid(grid1, "PLANTCODE"            , "공장"      , true, GridColDataType_emu.VarChar, 130, 130, Infragistics.Win.HAlign.Left, true, false);
                _GridUtil.InitColumnUltraGrid(grid1, "WORKER"               , "작업자"    , true, GridColDataType_emu.VarChar, 130, 130, Infragistics.Win.HAlign.Left, true, false);
                _GridUtil.InitColumnUltraGrid(grid1, "PRODDATE"             , "생산 일자" , true, GridColDataType_emu.VarChar, 130, 130, Infragistics.Win.HAlign.Left, true, false);
                _GridUtil.InitColumnUltraGrid(grid1, "WORKCENTERCODE"       , "작업장"    , true, GridColDataType_emu.VarChar, 130, 130, Infragistics.Win.HAlign.Left, true, false);
                _GridUtil.InitColumnUltraGrid(grid1, "WORKCENTERCODENAME"   , "작업장명"  , true, GridColDataType_emu.VarChar, 130, 130, Infragistics.Win.HAlign.Left, true, false);
                _GridUtil.InitColumnUltraGrid(grid1, "ITEMCODE"             , "품목"      , true, GridColDataType_emu.VarChar, 130, 130, Infragistics.Win.HAlign.Left, true, false);
                _GridUtil.InitColumnUltraGrid(grid1, "ITEMNAME"             , "품명"      , true, GridColDataType_emu.VarChar, 130, 130, Infragistics.Win.HAlign.Left, true, false);
                _GridUtil.InitColumnUltraGrid(grid1, "PRODQTY"              , "생산수량"  , true, GridColDataType_emu.Double , 130, 130, Infragistics.Win.HAlign.Right, true, false);
                _GridUtil.InitColumnUltraGrid(grid1, "BADQTY"               , "불량수량"  , true, GridColDataType_emu.Double , 130, 130, Infragistics.Win.HAlign.Right, true, false);
                _GridUtil.InitColumnUltraGrid(grid1, "TOTALQTY"             , "총생산량"  , true, GridColDataType_emu.Double , 130, 130, Infragistics.Win.HAlign.Right, true, false);
                _GridUtil.InitColumnUltraGrid(grid1, "BADRATIO"             , "불량율"    , true, GridColDataType_emu.VarChar , 130, 130, Infragistics.Win.HAlign.Right, true, false);
                _GridUtil.InitColumnUltraGrid(grid1, "MAKEDATE"             , "생산일시"  , true, GridColDataType_emu.VarChar, 130, 130, Infragistics.Win.HAlign.Left, true, false);
                // 그리드 바인딩
                _GridUtil.SetInitUltraGridBind(grid1);

                //콤보 박스 셋팅
                Common _Common = new Common();
                DataTable dtTemp = new DataTable();
                //PLANTCODE 기준정보 가져와서 데이터 테이블에 추가

                dtTemp = _Common.Standard_CODE("PLANTCODE");
                //데이터 테이블에 있는 데이터를 해당 콤보박스에 추가
                Common.FillComboboxMaster(this.cboPlantCode_H, dtTemp, dtTemp.Columns["CODE_ID"].ColumnName, dtTemp.Columns["CODE_NAME"].ColumnName, "ALL", "");
                UltraGridUtil.SetComboUltraGrid(this.grid1, "PLANTCODE", dtTemp, "CODE_ID", "CODE_NAME");

                BizTextBoxManager btbManager = new BizTextBoxManager();
                btbManager.PopUpAdd(txtWorkerID, txtWorkerName, "WORKER_MASTER", new object[] { "", "", "", "", "" });

            }
            catch (Exception ex)
            {

                ShowDialog(ex.Message, DC00_WinForm.DialogForm.DialogType.OK);
            }
        }

        public override void DoInquire()
        {
            base.DoInquire();
            DBHelper helper = new DBHelper(false);
            try
            {
                string sPlantCode = Convert.ToString(cboPlantCode_H.Value);
                string sWorkerName = Convert.ToString(txtWorkerName.Text);
                string sStartDate = string.Format("{0:yyyy-MM-dd}", dtpStart.Value);
                string sEndDate = string.Format("{0:yyyy-MM-dd}", dtpEnd.Value);

                DataTable dtTemp = new DataTable();
                dtTemp = helper.FillTable("06PP_WorkerPerProd_S1", CommandType.StoredProcedure, helper.CreateParameter("PLANTCODE"  , sPlantCode, DbType.String, ParameterDirection.Input)
                                                                                             , helper.CreateParameter("WORKER"      , sWorkerName, DbType.String, ParameterDirection.Input)
                                                                                             , helper.CreateParameter("STARTDATE"   , sStartDate, DbType.String, ParameterDirection.Input)
                                                                                             , helper.CreateParameter("ENDDATE"     , sEndDate, DbType.String, ParameterDirection.Input));

                this.ClosePrgForm();

                this.grid1.DisplayLayout.Override.MergedCellContentArea = Infragistics.Win.UltraWinGrid.MergedCellContentArea.VirtualRect;
                this.grid1.DisplayLayout.Bands[0].Columns["PLANTCODE"].MergedCellStyle = Infragistics.Win.UltraWinGrid.MergedCellStyle.Always;
                this.grid1.DisplayLayout.Bands[0].Columns["WORKER"].MergedCellStyle = Infragistics.Win.UltraWinGrid.MergedCellStyle.Always;
                this.grid1.DisplayLayout.Bands[0].Columns["PRODDATE"].MergedCellStyle = Infragistics.Win.UltraWinGrid.MergedCellStyle.Always;
                this.grid1.DisplayLayout.Bands[0].Columns["WORKCENTERCODE"].MergedCellStyle = Infragistics.Win.UltraWinGrid.MergedCellStyle.Always;
                this.grid1.DisplayLayout.Bands[0].Columns["WORKCENTERCODENAME"].MergedCellStyle = Infragistics.Win.UltraWinGrid.MergedCellStyle.Always;
                this.grid1.DisplayLayout.Bands[0].Columns["ITEMCODE"].MergedCellStyle = Infragistics.Win.UltraWinGrid.MergedCellStyle.Always;
                this.grid1.DisplayLayout.Bands[0].Columns["ITEMNAME"].MergedCellStyle = Infragistics.Win.UltraWinGrid.MergedCellStyle.Always;

                if (dtTemp.Rows.Count !=0)
                {
                    // SUB-TOTAL
                    DataTable dtSubTotal = dtTemp.Clone(); // 데이터 테이블의 서식 복사

                    string sWorkerRow = Convert.ToString(dtTemp.Rows[0]["WORKER"]);
                    double sProdQty = Convert.ToDouble(dtTemp.Rows[0]["PRODQTY"]);
                    double sBadQty = Convert.ToDouble(dtTemp.Rows[0]["BADQTY"]);
                    double sTotalQty = Convert.ToDouble(dtTemp.Rows[0]["TOTALQTY"]);

                    dtSubTotal.Rows.Add(new object[] { Convert.ToString(dtTemp.Rows[0]["PLANTCODE"]),
                                                       Convert.ToString(dtTemp.Rows[0]["WORKER"]),
                                                       Convert.ToString(dtTemp.Rows[0]["PRODDATE"]),
                                                       Convert.ToString(dtTemp.Rows[0]["WORKCENTERCODE"]),
                                                       Convert.ToString(dtTemp.Rows[0]["WORKCENTERCODENAME"]),
                                                       Convert.ToString(dtTemp.Rows[0]["ITEMCODE"]),
                                                       Convert.ToString(dtTemp.Rows[0]["ITEMNAME"]),
                                                       Convert.ToString(dtTemp.Rows[0]["PRODQTY"]),
                                                       Convert.ToString(dtTemp.Rows[0]["BADQTY"]),
                                                       Convert.ToString(dtTemp.Rows[0]["TOTALQTY"]),
                                                       Convert.ToString(dtTemp.Rows[0]["BADRATIO"]),
                                                       Convert.ToString(dtTemp.Rows[0]["MAKEDATE"]),            
                                                     });
                    for (int i = 1; i < dtTemp.Rows.Count; i++)
                    {
                        if (sWorkerRow == Convert.ToString(dtTemp.Rows[i]["WORKER"]))
                        {
                            sProdQty = sProdQty + Convert.ToDouble(dtTemp.Rows[i]["PRODQTY"]);
                            sBadQty = sBadQty + Convert.ToDouble(dtTemp.Rows[i]["BADQTY"]);
                            sTotalQty = sTotalQty + Convert.ToDouble(dtTemp.Rows[i]["TOTALQTY"]);

                            dtSubTotal.Rows.Add(new object[] { Convert.ToString(dtTemp.Rows[0]["PLANTCODE"]),
                                                       Convert.ToString(dtTemp.Rows[i]["WORKER"]),
                                                       Convert.ToString(dtTemp.Rows[i]["PRODDATE"]),
                                                       Convert.ToString(dtTemp.Rows[i]["WORKCENTERCODE"]),
                                                       Convert.ToString(dtTemp.Rows[i]["WORKCENTERCODENAME"]),
                                                       Convert.ToString(dtTemp.Rows[i]["ITEMCODE"]),
                                                       Convert.ToString(dtTemp.Rows[i]["ITEMNAME"]),
                                                       Convert.ToString(dtTemp.Rows[i]["PRODQTY"]),
                                                       Convert.ToString(dtTemp.Rows[i]["BADQTY"]),
                                                       Convert.ToString(dtTemp.Rows[i]["TOTALQTY"]),
                                                       Convert.ToString(dtTemp.Rows[i]["BADRATIO"]),
                                                       Convert.ToString(dtTemp.Rows[i]["MAKEDATE"]),
                                                      });
                            continue;

                        }
                        else
                        {
                            dtSubTotal.Rows.Add(new object[] { "", "", "", "", "", "", "합   계 :", sProdQty, sBadQty, sTotalQty, Convert.ToString(Math.Round(sBadQty * 100 / sTotalQty, 1)) + " %", "" });
                            sProdQty =  Convert.ToDouble(dtTemp.Rows[i]["PRODQTY"]);
                            sBadQty =   Convert.ToDouble(dtTemp.Rows[i]["BADQTY"]);
                            sTotalQty = Convert.ToDouble(dtTemp.Rows[i]["TOTALQTY"]);

                            dtSubTotal.Rows.Add(new object[] { Convert.ToString(dtTemp.Rows[0]["PLANTCODE"]),
                                                       Convert.ToString(dtTemp.Rows[i]["WORKER"]),
                                                       Convert.ToString(dtTemp.Rows[i]["PRODDATE"]),
                                                       Convert.ToString(dtTemp.Rows[i]["WORKCENTERCODE"]),
                                                       Convert.ToString(dtTemp.Rows[i]["WORKCENTERCODENAME"]),
                                                       Convert.ToString(dtTemp.Rows[i]["ITEMCODE"]),
                                                       Convert.ToString(dtTemp.Rows[i]["ITEMNAME"]),
                                                       Convert.ToString(dtTemp.Rows[i]["PRODQTY"]),
                                                       Convert.ToString(dtTemp.Rows[i]["BADQTY"]),
                                                       Convert.ToString(dtTemp.Rows[i]["TOTALQTY"]),
                                                       Convert.ToString(dtTemp.Rows[i]["BADRATIO"]),
                                                       Convert.ToString(dtTemp.Rows[i]["MAKEDATE"]),
                                                      });
                            sWorkerRow = Convert.ToString(dtTemp.Rows[i]["WORKER"]);
                        }
                    }
                    dtSubTotal.Rows.Add(new object[] { "", "", "", "", "", "", "합   계 :", sProdQty, sBadQty, sTotalQty, Convert.ToString(Math.Round(sBadQty * 100 / sTotalQty, 1)) + " %", "" });
                    this.grid1.DataSource = dtSubTotal;
                }
/*                else
                {
                    _GridUtil.Grid_Clear(grid1);
                    ShowDialog("조회할 데이터가 없습니다.", DC00_WinForm.DialogForm.DialogType.OK);
                }*/
            }
            catch (Exception ex)
            {

                ShowDialog(ex.Message, DC00_WinForm.DialogForm.DialogType.OK);
            }
            finally
            {
                helper.Close();
            }
        }
    }
}
