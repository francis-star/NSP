
var Address = "../"; //当前的html与Aspx中PubForm的文件深度

$(function () {

    var data = eval("(" + ReturnValue("BLL_PublicInfo", "GetList", "", Address) + ")");
    var content, code;
    for (var i = 0; i < data.length; i++) {
        var arr = new Array();
        code = data[i].Pub_Code;
        if (code == "2016111000054") {
            content = escape(decodeURIComponent(data[i].Pub_Content));
            arr[0] = code;
            arr[1] = content;
            ReturnValue("BLL_PublicInfo", "UpdateContent", arr, Address)
        }
    }

});
