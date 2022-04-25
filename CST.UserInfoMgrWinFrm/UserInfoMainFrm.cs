using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CST.UserInfoMgrWinFrm
{
    public partial class UserInfoMainFrm : Form
    {
        //define global variable updateUserID
        private int updateUserID = 0;
        public UserInfoMainFrm()
        {
            InitializeComponent();
        }

        private void UserInfoMainFrm_Load(object sender, EventArgs e)
        {
            // load database data to gridview

            //LoadUserInfos();

            this.dgvUserInfo.DataSource = LoadUserInfos();
        }

        #region method Loaduserinfos return userlist

        private List<UserInfo> LoadUserInfos()

        {
            this.dgvUserInfo.AutoGenerateColumns = false;
            string sql = "select UserID,UserName,UserPassword,UserAge,UserMobile,UserEmail,CreateDate from UserInfo";
            DataTable da = SqlHelper.GetDataTable(sql, CommandType.Text); // no parameter need
            List<UserInfo> list = null;
            if (da.Rows.Count > 0)
            {
                list = new List<UserInfo>();
                UserInfo userInfo = null;
                foreach (DataRow row in da.Rows)

                {
                    userInfo = new UserInfo();

                    LoadEntity(userInfo, row);

                    list.Add(userInfo);

                }



            }

            return list;
        }
        #endregion


        #region method LoadEntity
        /// <summary>
        /// load entity 
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="row"></param>
        private void LoadEntity(UserInfo userInfo, DataRow row)
        {
            userInfo.UserID = Convert.ToInt32(row["UserID"]);
            userInfo.Username = row["UserName"] != DBNull.Value ? row["UserName"].ToString() : string.Empty;
            userInfo.UserPassword = row["UserPassword"].ToString();
            userInfo.UserMobile = row["UserMobile"] != DBNull.Value ? row["UserMobile"].ToString() : string.Empty;
            //userInfo.CreateDate = row["CreateDate"]!= DBNull.Value ? Convert.ToDateTime(row["CreateDate"]) : SqlDateTime.MinValue.ToString();
            userInfo.CreateDate = DateTime.Parse(row["CreateDate"] == DBNull.Value ? SqlDateTime.MinValue.ToString() : row["CreateDate"].ToString());
            userInfo.UserEmail = row["UserEmail"] != DBNull.Value ? row["UserEmail"].ToString() : string.Empty;

            userInfo.UserAge = row["UserAge"] != DBNull.Value ? Convert.ToInt32(row["UserAge"].ToString()) : 18;



        }

        #endregion


        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #region Delete method
        private void btnDelete_Click(object sender, EventArgs e)
        {
            // get User id of delted item

            if (this.dgvUserInfo.SelectedRows.Count <= 0)
            {
                MessageBox.Show("please select");
                return;
            }


            if (MessageBox.Show("Confirm delete", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) !=
                    DialogResult.Yes)
            {
                return;
            }

            //MessageBox.Show("Deleted successfully!!");



            //MessageBox.Show(this.dgvUserInfo.SelectedRows[0].Cells["UserID"].Value.ToString());

            int deleteUserID = int.Parse(this.dgvUserInfo.SelectedRows[0].Cells["UserID"].Value.ToString());

            //string sql = "delete from UserInfo where UserId=@UserID";

            string sql = "update UserInfo set DelFlag=1 where UserID=@UserID";


            SqlParameter[] pars =
            {
                new SqlParameter("@UserID",(object) deleteUserID)
            };

            int nums = SqlHelper.ExecuteNoQuery(sql, CommandType.Text, pars);

            if (nums > 0)
            {
                MessageBox.Show("Deleted successfully!!");
                LoadUserInfos();
            }

            else
            {
                MessageBox.Show("Delete Failure");
            }
            //SqlHelper.ExecuteNoQuery(sql, CommandType.Text, new SqlParameter("UserID", (object)deleteUserID));



            //
        }
        #endregion

        #region method update userinfo
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string sql = "update Userinfo set UserName=@UserName,UserAge=@UserAge,UserMobile=@UserMobile,Useremail=@UserEmail where UserID=@UserID";

            SqlParameter[] pars =
                {
                   new SqlParameter("UserID",SqlDbType.Int),
                   new SqlParameter("UserName",SqlDbType.VarChar,50),
                  //new SqlParameter("UserPassword",SqlDbType.VarChar,50),
                   new SqlParameter("UserAge",SqlDbType.Int),
                   new SqlParameter("UserMobile",SqlDbType.VarChar,20),
                   new SqlParameter("UserEmail",SqlDbType.VarChar,100),

            };

            pars[0].Value = updateUserID;
            pars[1].Value = this.txtUsername.Text;
            pars[2].Value = Convert.ToInt32(this.txtAge.Text);
            pars[3].Value = this.txtMobile.Text;
            pars[4].Value = this.txtUserEmail.Text;

            int nums = SqlHelper.ExecuteNoQuery(sql, CommandType.Text, pars);

            if (nums >= 1)
            {
                MessageBox.Show("Update success");


                // need referesh data table again

                this.dgvUserInfo.AutoGenerateColumns = false;
                this.dgvUserInfo.DataSource = LoadUserInfos();


            }
            else
            {
                MessageBox.Show("Update failure");
            }


        } 
        #endregion




        #region load detailed userinfo by slected id, fill to text box--selection changed
        private void dgvUserInfo_SelectionChanged(object sender, EventArgs e)
        {
            if (this.dgvUserInfo.SelectedRows.Count <= 0)
            {
                return;
            }

            //MessageBox.Show(this.dgvUserInfo.SelectedRows[0].Cells[0].Value.ToString());

            int selectedUserID = int.Parse(this.dgvUserInfo.SelectedRows[0].Cells[0].Value.ToString());

            // assign id to global updateUSerID

            updateUserID = selectedUserID;

            DataTable dt = GetUserInfo(selectedUserID);
            UserInfo userInfo = null;
            if (dt.Rows.Count > 0)

            {
                userInfo = new UserInfo();

                LoadEntity(userInfo, dt.Rows[0]);

            }

            this.txtUsername.Text = userInfo.Username;
            this.txtMobile.Text = userInfo.UserMobile;
            this.txtUserEmail.Text = userInfo.UserEmail.ToString();
            this.txtCreateDate.Text = userInfo.CreateDate.ToString();
            this.txtAge.Text = userInfo.UserAge.ToString();






        }
        #endregion

        #region method getuserinfo by ID
        private DataTable GetUserInfo(int id)
        {
            string sql = "select UserID,UserName,UserPassword,UserAge,UserMobile,UserEmail,CreateDate from Userinfo where UserID=@UserID";

            SqlParameter[] pars = {

                 new SqlParameter("@UserID",SqlDbType.Int),

            };

            pars[0].Value = id;

            DataTable dt = SqlHelper.GetDataTable(sql, CommandType.Text, pars);

            return dt;
        }
 #endregion
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string sqlText = "select UserID,UserName,UserAge,UserMobile,UserEmail,UserPassword,CreateDate from UserInfo ";
            List<string> whereList = new List<string>();

            List<SqlParameter> parameters = new List<SqlParameter>();

            // first condition
            if (!string.IsNullOrEmpty(this.txtSearchUserName.Text.Trim()))

            {
                whereList.Add(" UserName Like @UserName ");
                SqlParameter parameter = new SqlParameter();
                parameter.ParameterName = "@UserName";
                parameter.Value = "%" + txtSearchUserName.Text + "%";
                parameters.Add(parameter);

            
            }

            // second condition
            if (!string.IsNullOrEmpty(this.txtSearchMobile.Text.Trim()))

            {
                 // add condition to where list collection

                whereList.Add(" UserMobile Like @UserMobile ");
                SqlParameter parameter = new SqlParameter();
                parameter.ParameterName = "@UserMobile";
                parameter.Value = "%" + txtSearchMobile.Text + "%";
                parameters.Add(parameter);


            }

            if (whereList.Count > 0)
            {
               sqlText += " where "+string.Join(" and ",whereList);
            }

            DataTable dt = SqlHelper.GetDataTable(sqlText, CommandType.Text, parameters.ToArray());

            List<UserInfo> userList = null;

            if (dt.Rows.Count > 0)
            {
                userList = new List<UserInfo>();

                UserInfo userInfo = null;

                foreach (DataRow row in dt.Rows)
                {
                    userInfo = new UserInfo();
                    LoadEntity(userInfo, row);

                    userList.Add(userInfo);
                }
            
            }

            this.dgvUserInfo.AutoGenerateColumns = false;
            this.dgvUserInfo.DataSource = userList;


        
        //
        }





        //

    }
}
