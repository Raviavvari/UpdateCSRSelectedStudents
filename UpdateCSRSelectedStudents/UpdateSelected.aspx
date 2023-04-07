<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UpdateSelected.aspx.cs" Inherits="UpdateCSRSelectedStudents.UpdateSelected" EnableEventValidation="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script src="jquery-3.6.3.min.js"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.6.3/jquery.min.js"></script>
    <title></title>
    <script type="text/javascript" src="../Scripts/bootstrap.min.js"></script>
    <script type="text/javascript">        
        function onFileNameChange(input) {
            debugger;
            var tbfilePath = $('#<%=tbfileup.ClientID%>');
            tbfilePath.val(fileuploadctrl.files[0].name);
        }
        function showBrowseDialog() {
            fileuploadctrl = document.getElementById('<%=tbFile_Upload.ClientID%>');
            fileuploadctrl.click();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="row" style="padding-top:20px;">
            <div class="form-group">
                <asp:TextBox ID="tbfileup" runat="server" ReadOnly="true" CssClass="col-lg-4 form-control" Width="650px" Style="flex-grow: 1" Placeholder="Select your file"></asp:TextBox>                                
                <asp:RequiredFieldValidator ID="reqvalid" Text="required" ForeColor="Red" runat="server" ControlToValidate="tbFile_Upload" Display="Dynamic"></asp:RequiredFieldValidator>
                <asp:Button ID="btnbrowse" CausesValidation="false" OnClientClick="showBrowseDialog(); return false;" Text="Browse" style="text-align:center" Width="150px" CssClass="col-lg-2 btn btn-primary" runat="server"/>
                <asp:Button ID="btn_upload" Text="Upload" CssClass="btn btn-success text-center" runat="server" OnClick="btn_upload_Click" Width="150px"/>
                            <%--  &nbsp;&nbsp;--%>
            </div>
            <div>
                <asp:FileUpload ID="tbFile_Upload" onchange="onFileNameChange(this)" Style="display: none" runat="server" />
            </div>
        </div>
    </form>
</body>
</html>
