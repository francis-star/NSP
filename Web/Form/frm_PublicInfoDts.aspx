<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frm_PublicInfoDts.aspx.cs" Inherits="Web.Form.frm_PublicInfoDts" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>资讯信息维护</title>

    
    <link href="../JS/lib/ligerUI/skins/Aqua/css/ligerui-all.css" rel="stylesheet" type="text/css" />
    <link href="../JS/lib/ligerUI/skins/gray2014/css/all.css" rel="stylesheet" type="text/css" />
    <link href="../JS/lib/ligerUI/skins/ligerui-icons.css" rel="stylesheet" type="text/css" />
    <link type="text/css" rel="stylesheet" href="../CSS/box.css" />
    <script src="../JS/lib/jquery/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script src="../JS/lib/ligerUI/js/core/base.js" type="text/javascript"></script>
    <script src="../JS/lib/ligerUI/js/ligerui.all.js" type="text/javascript"></script>
    <script src="../JS/artDialog4.1.4/artDialog.source.js?skin=default" type="text/javascript"></script>
    <script src="../JS/artDialog4.1.4/plugins/iframeTools.source.js" type="text/javascript"></script>
    <script src="../JS/HCWeb2016.js"></script>
    <script src="../JS/GetQueryString.js" type="text/javascript"></script>
    <script src="../JSHC/PublicInfoDts.js"></script>
    <script src="../JSHC/Pub.js" type="text/javascript"></script>
    <script type="text/javascript" src="../JS/ueditor/ueditor.config.js"></script>
    <script type="text/javascript" src="../JS/ueditor/ueditor.all.js"></script>
    <script type="text/javascript" src="../JS/ueditor/lang/zh-cn/zh-cn.js"></script>
    <script language="javascript" type="text/javascript">
        // <!CDATA[

        function Uploadify_onclick(paramTbid) {
            if ($("#" + paramTbid).next().val() == "上传") {
                art.dialog.data('Path', paramTbid); //接收返回路径
                art.dialog.data('FileType', '*.jpg;*.gif;*.png;*.jpeg;*.bmp'); //文件类型
                art.dialog.data('Filedata', '100'); //文件大小，单位M
                OpenForm("../UpFile/UpFile.htm", "上传文件", "500px", "150px"); //窗体大小
            }
            else {
                $("#" + paramTbid).val("");
                $("#" + paramTbid).next().val("上传");
            }
        }

        // ]]>
    </script>
    <style>
        .l-text-wrapper {
            position: relative;
            float: left;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <div id="toptoolbar">
    </div>
    <div id="SearchDiv" class="SearchDiv" style="margin-left: 5px; margin-top: 5px; width: 99%">
        <table  class="bor1 w1 b-l-p2">
            <tr class="b-l-p">
                <td class="b-bg w10 t-c">
                    移动端  
                </td>
                <td class="w40">
                    <input id="MobileTerminal" type="text" runat="server" />
                </td>
                <td class="b-bg w10 t-c">
                    平台名称  
                </td>
                <td class="w40">
                    <input id="PlatformName" type="text" runat="server" />
                </td>
            </tr>
            <tr class="b-l-p">
                <td class="b-bg w10 t-c">
                    信息类别  
                </td>
                <td class="w40">
                    <input id="InformationCategory" type="text" runat="server" />
                </td>
                <td class="b-bg w10 t-c">
                    信息小类  
                </td>
                <td class="w40">
                    <input id="InformationType" type="text" runat="server" />
                </td>
            </tr>
            <tr class="b-l-p">
                <td class="b-bg w10 t-c">
                    地区  
                </td>
                <td class="w40">
                    <input id="Province" type="text" runat="server"/> <input id="City" type="text" runat="server"/> <input id="District" type="text" runat="server" />
                </td>
                <td class="b-bg w10 t-c" >
                    关键字  
                </td>
                <td class="w40">
                    <input id="KeyWords" type="text" runat="server" />
                </td>
            </tr>
            <tr class="b-l-p">
                <td class="b-bg w10 t-c" >
                    标题  
                </td>
                <td class="w40">
                    <input id="Inp_Title" type="text" runat="server" />
                </td>
                <td class="b-bg w10 t-c">
                    文章出处  
                </td>
                <td class="w40">
                    <input id="Inp_ComeFrom" type="text" runat="server"/>
                </td>
            </tr>
            <tr class="b-l-p">
                <td class="b-bg w10 t-c">
                    封面图片  
                </td>
                <td class="w40" colspan="3">                    
                    <input id="PicPath1" type="text" class="in-text w40" value="" runat="server" readonly="readonly" onfocus="return Uploadify_onclick('PicPath1')" />
                    <input id="Button1" type="button" value="上传" style="width:5%" onclick="return Uploadify_onclick('PicPath1')"" />
                    <input id="PicPath2" type="text" class="in-text w40" value="" runat="server" readonly="readonly"  onfocus="return Uploadify_onclick('PicPath1')" />
                    <input id="Button2" type="button" value="上传" style="width:5%" onclick="return Uploadify_onclick('PicPath2')"" />
                    <input id="PicPath3" type="text" class="in-text w40" value="" runat="server" readonly="readonly"  onfocus="return Uploadify_onclick('PicPath1')" />
                    <input id="Button3" type="button" value="上传" style="width:5%" onclick="return Uploadify_onclick('PicPath3')"" />
                </td>
            </tr>
            <tr class="b-l-p">
                <td class="b-bg w10 t-c">
                    内容  
                </td>
                <td class="w40" colspan="3">
                    <asp:TextBox ID="txtContent" runat="server" TextMode="MultiLine"></asp:TextBox>

                    <script type="text/javascript">
                        window.UEDITOR_HOME_URL = 'ueditor/';
                        var editor = new UE.ui.Editor({ minFrameHeight: 250, initialFrameHeight: 250, initialFrameWidth: 1000, textarea: 'txtContent', initialContent: '' });
                        editor.render('txtContent');
                    </script>
                </td>
            </tr>
        </table>
    </div>
    </div>
    </form>
</body>
</html>
