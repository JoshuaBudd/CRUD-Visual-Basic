Imports System.Data
Imports System.Data.SqlClient

Public Class Form1

    Private Function getConnectionString() As String
        Return My.Settings.peopleConnectionString
    End Function

    Private conn As New SqlConnection(getConnectionString())
    Private da As New SqlDataAdapter("Select * from Peoples", conn)
    Private ds As New DataSet("PeopleDS")
    Private dt As New DataTable("Peoples")

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ds.Tables.Add(dt)
        da.Fill(ds.Tables("Peoples"))
        DataGridView1.DataSource = ds.Tables("Peoples")
        Dim commands As New SqlCommandBuilder(da)
    End Sub

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        ds.EndInit()
        da.Update(ds.Tables("Peoples"))
    End Sub

    Private Sub AddPeople(fname As String, lname As String, id As String)
        Dim sql As String
        Dim conn As SqlConnection
        Dim cmd As SqlCommand
        sql = "insert into peoples(firstname, lastname, people_ID) values(@firstname, @lastname, @people_ID)"
        conn = New SqlConnection(getConnectionString())
        conn.Open()
        cmd = New SqlCommand(sql, conn)
        cmd.Parameters.AddWithValue("@lastname", lname)
        cmd.Parameters.AddWithValue("@firstname", fname)
        cmd.Parameters.AddWithValue("@people_ID", id)
        cmd.ExecuteNonQuery()
        conn.Close()
    End Sub


    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Try
            AddPeople(txtFirstName.Text, txtLastName.Text, txtPeopleID.Text)
            MessageBox.Show("Added")
        Catch ex As Exception
            MessageBox.Show("Error" & ex.Message)
        End Try
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        Dim sql As String
        Dim conn As SqlConnection
        Dim cmd As SqlCommand
        sql = "delete from peoples where people_ID=@people_ID"
        conn = New SqlConnection(getConnectionString())
        conn.Open()
        cmd = New SqlCommand(sql, conn)
        cmd.Parameters.AddWithValue("@people_ID", CInt(txtPeopleID.Text))
        cmd.ExecuteNonQuery()
        conn.Close()
        btnUpdate.PerformClick()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        Application.Exit()
    End Sub

    Private Sub btnFilter_Click(sender As Object, e As EventArgs) Handles btnFilter.Click
        Dim dv As New DataView(ds.Tables(0))
        dv.RowFilter = "people_ID like'%" & TextBox1.Text & "%'"
        DataGridView1.DataSource = dv
    End Sub

    Private Sub btnSort_Click(sender As Object, e As EventArgs) Handles btnSort.Click
        Dim dv As New DataView(ds.Tables(0))
        DataGridView1.DataSource = dv
        dv.Sort = "people_ID"
    End Sub
End Class
