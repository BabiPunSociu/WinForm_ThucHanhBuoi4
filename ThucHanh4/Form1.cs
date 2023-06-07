using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ThucHanh4
{
    public partial class Form1 : Form
    {
        DataBaseProcess dtbase = new DataBaseProcess();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Load du lieu lên dgvKetQua
            dgvKetQua.DataSource = dtbase.DataReader("Select * from tblMatHang");
            // Ẩn nút sửa, xóa
            btnSua.Visible = false;
            btnXoa.Visible = false;
            // Ẩn grbChiTiet
            grbChiTiet.Enabled = false;
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            // Kiểm tra input
            if (txtMaSP.Text.Trim() == "" && txtTenSP.Text.Trim() == "")
            {
                MessageBox.Show("Vui lòng nhập MaSP hoặc TenSP cần tìm kiếm!");
                txtMaSP.Focus();
                return;
            }
            // Cập nhật tiêu đề
            lblTieuDe.Text = "TÌM KIẾM MẶT HÀNG";
            // Ẩn nút sửa, xóa
            btnSua.Visible = false;
            btnXoa.Visible = false;
            string sql = "Select * from tblMatHang where ";
            // Tìm theo mã sản phẩm
            if (txtMaSP.Text.Trim() != "" && txtMaSP.Text.Trim() != "")
            {
                sql += " MaSP like N'" + txtMaSP.Text.Trim() + "%' and TenSP like N'" + txtTenSP.Text.Trim() + "%'";

            }
            else
            {
                if (txtMaSP.Text.Trim() != "")
                    sql += " MaSP like N'" + txtMaSP.Text.Trim() + "%'";
                else
                    sql += " TenSP like N'" + txtTenSP.Text.Trim() + "%'";
            }

            // Load dữ liệu lên dgv
            dgvKetQua.DataSource = dtbase.DataReader(sql);
        }

        private void dgvKetQua_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Hiện nút sửa xóa
            btnSua.Visible = true;
            btnXoa.Visible = true;
            btnThem.Visible = false;
            // Bắt lỗi người dùng click linh tinh trên dgv
            try
            {
                txtMaSP1.Text = dgvKetQua.CurrentRow.Cells[0].Value.ToString();
                txtTenSP1.Text = dgvKetQua.CurrentRow.Cells[1].Value.ToString();
                dtpSX.Value = (DateTime)dgvKetQua.CurrentRow.Cells[2].Value;
                dtpHH.Value = (DateTime)dgvKetQua.CurrentRow.Cells[3].Value;
                txtDonVi.Text = dgvKetQua.CurrentRow.Cells[4].Value.ToString();
                txtDonGia.Text = dgvKetQua.CurrentRow.Cells[5].Value.ToString();
                txtGhiChu.Text = dgvKetQua.CurrentRow.Cells[6].Value.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Reset_grbChiTiet()
        {
            txtMaSP1.Clear();
            txtTenSP1.Clear();
            dtpSX.Value = DateTime.Today;
            dtpHH.Value = DateTime.Today;
            txtDonGia.Clear();
            txtDonVi.Clear();
            txtGhiChu.Clear();
        }
        private void btnThem_Click(object sender, EventArgs e)
        {
            /*   reset grbChiTiet   */
            Reset_grbChiTiet();

            // Ẩn nút sửa xóa
            btnSua.Visible = false;
            btnXoa.Visible = false;

            // Enable grbChiTiet
            grbChiTiet.Enabled = true;
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            lblTieuDe.Text = "CẬP NHẬT MẶT HÀNG";
            btnThem.Visible = false;
            btnXoa.Visible = false;

            grbChiTiet.Enabled = true;
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            // Message Box cảnh báo người dùng
            if (MessageBox.Show("Bạn có chắc chắn xóa mặt hàng " + txtMaSP1.Text.Trim() + " không?", "Xóa sản phẩm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                lblTieuDe.Text = "XÓA MẶT HÀNG";
                btnThem.Visible = false;
                btnSua.Visible = false;

                grbChiTiet.Enabled = true;
                btnLuu_Click(sender,e);
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string sql = "";
            // Sử dụng control ErrProvider để hiển thị lỗi
            if (txtMaSP1.Text.Trim() == "")
            {
                errChiTiet.SetError(txtMaSP1, "Bạn không được để trống mã sản phẩm.");
                return;
            }
            else
                errChiTiet.Clear();

            if (txtTenSP1.Text.Trim() == "")
            {
                errChiTiet.SetError(txtTenSP1, "Bạn không được để trống tên sản phẩm.");
                return;
            }
            else
                errChiTiet.Clear();

            if (dtpSX.Value > DateTime.Now)
            {
                errChiTiet.SetError(dtpSX, "Ngày sản xuất không hợp lệ");
                return;
            }
            else
                errChiTiet.Clear();
            if (dtpSX.Value > dtpHH.Value)
            {
                errChiTiet.SetError(dtpHH, "Ngày hết hạn nhỏ hơn ngày sản xuất");
                return;
            }
            else
                errChiTiet.Clear();

            if (txtDonVi.Text.Trim() == "")
            {
                errChiTiet.SetError(txtDonVi, "Bạn không được để trống đơn vị");
                return;
            }
            else
                errChiTiet.Clear();

            if (txtDonGia.Text.Trim() == "")
            {
                errChiTiet.SetError(txtDonGia, "Bạn không được để trống đơn giá");
                return;
            }
            else
                errChiTiet.Clear();

            if (btnThem.Visible == true) // Thêm mới
            {
                // Kiểm tra maSP đã tồn tại chưa
                sql = "Select * from tblMatHang where MaSP = N'" + txtMaSP1.Text.Trim() + "'";
                DataTable dt = dtbase.DataReader(sql);
                if (dt.Rows.Count > 0)
                {
                    errChiTiet.SetError(txtMaSP1, "Mã sản phẩm đã tồn tại, hãy thử lại mã khác");
                    return;
                }
                errChiTiet.Clear();

                // Insert
                /*
                sql = "Insert into tblMatHang(MaSP, TenSP, NgaySX, NgayHH, DonVi, DonGia, GhiChu) VALUES "
                    + " (N'" + txtMaSP1.Text.Trim() + "',N'" + txtTenSP1 + "', " + dtpSX.Value.Date + "," + dtpHH.Value.Date
                    + ", N'" + txtDonVi.Text.Trim() + "', " + txtDonGia.Text.Trim() + ",N'" + txtGhiChu.Text.Trim() + "'";
                */
                sql = "INSERT INTO tblMatHang(MaSP, TenSP, NgaySX, NgayHH, DonVi, DonGia, GhiChu) VALUES(";
                sql += "N'" + txtMaSP1.Text + "',N'" + txtTenSP1.Text + "','" + dtpSX.Value.Date + "','" +
                     dtpHH.Value.Date + "',N'" + txtDonVi.Text + "',N'" +
                     txtDonGia.Text + "',N'" + txtGhiChu.Text + "')";
            }
            if (btnSua.Visible == true)  // Sửa
            {
                
                sql = "Update tblMatHang SET ";
                sql += "TenSP = N'" + txtTenSP1.Text.Trim() + "',";
                sql += "NgaySX = '" + dtpSX.Value.Date + "',";
                sql += "NgayHH = '" + dtpHH.Value.Date + "',";
                sql += "DonVi = N'" + txtDonVi.Text.Trim() + "',";
                sql += "DonGia = N'" + txtDonGia.Text.Trim() + "',";
                sql += "GhiChu = N'" + txtGhiChu.Text.Trim() + "' ";
                sql += "where MaSP = N'" + txtMaSP1.Text.Trim() + "'";
                

                /*
                sql = "Update tblMatHang SET ";
                sql += "TenSP = N'" + txtTenSP1.Text + "',";
                sql += "NgaySX = '" + dtpSX.Value.Date + "',";
                sql += "NgayHH = '" + dtpHH.Value.Date + "',";
                sql += "DonVi = N'" + txtDonVi.Text + "',";
                sql += "DonGia = '" + txtDonGia.Text + "',";
                sql += "GhiChu = N'" + txtGhiChu.Text + "' ";
                sql += "Where MaSP = N'" + txtMaSP1.Text + "'";
                */
            }
            if (btnXoa.Visible == true)  // Xóa
            {
                sql = "Delete from tblMatHang where MaSP = N'" + txtMaSP1.Text.Trim() + "'";
            }

            dtbase.Datachange(sql);

            // Cập nhật lại dvg
            sql = "Select * from tblMatHang";
            dgvKetQua.DataSource = dtbase.DataReader(sql);
            Reset_grbChiTiet();

            // Ẩn grbChiTiet và Sửa Xóa
            grbChiTiet.Enabled = false;
            btnSua.Visible = false;
            btnXoa.Visible = false;

            btnThem.Visible = true;
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            btnXoa.Visible = false;
            btnSua.Visible = false;

            btnThem.Visible = true;
            Reset_grbChiTiet();
            grbChiTiet.Enabled = false;
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn muốn thoát không?", "Thông báo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                Application.Exit();
        }
    }
}
