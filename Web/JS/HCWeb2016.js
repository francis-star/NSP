/*******************************/
/******户传js共用操作方法*******/
/*******************************/
/****2016.6.2******************/

$(function () {
    //创建遮罩div
    if (document.getElementById("mask") != null)
        document.body.removeChild(document.getElementById("mask"));
    var Div = document.createElement("div"); //创建div
    Div.setAttribute("id", "mask");
    var str = "<div class=\"mask\" style=\"display:none;\"><div class=\"loading\"></div></div>";
    Div.innerHTML = str;
    document.body.appendChild(Div);
})

//BLL名称，方法，参数数组，调用页面的相对路径，返回的方法名
function ReturnMethod(BLL, Method, Para, Address, ReturnMethodName) {
    $(".mask").show();
    setTimeout(function () {
        if (Para != null)
            ReplaceStr(Para);
        AjaxUrl(BLL, Method, Para, Address, ReturnMethodName)
        $(".mask").hide();
    }, 10);

}

//BLL名称，方法，参数数组，调用页面的相对路径，返回执行后的结果
function ReturnValue(BLL, Method, Para, Address) {
    if (Para != null)
        ReplaceStr(Para);
    var Result = AjaxUrlTwo(BLL, Method, Para, Address);
    return Result;

}

//针对一个传参时，如果出现参数为空时，自动补充一个空参数，且中文逗号取代英文逗号
function ReplaceStr(para) {
    //针对一个传参时，如果出现参数为空时，自动补充一个空参数
    if (para.length == 1 && para[0] == "")
        para[1] = "";
    if (para.length > 0) {
        for (var i = 0; i < para.length; i++) {
            if (para[i] != undefined && isNaN(para[i])) {
                para[i] = para[i].replace(/,/g, '，');
            }
        }
    }
}

//ajax数据调用
function AjaxUrl(BLL, Method, Para, Address, MethodName) {
    $.ajax({
        type: 'POST',
        cache: false,
        async: false,
        url: Address + 'Aspx/PubForm2016.aspx',
        data: Method + '臡' + BLL + '臡' + encodeURI(Para),
        success: function (text) {
            if (text == "Error") {
                ReturnErrorUrl();
            }
            CustomMethod[MethodName](text);
        }
    })
}


function AjaxUrlTwo(BLL, Method, Para, Address) {
    var Result = "";
    $.ajax({
        type: 'POST',
        cache: false,
        async: false,
        url: Address + 'Aspx/PubForm2016.aspx',
        data: Method + '臡' + BLL + '臡' + encodeURI(Para),
        success: function (text) {
            Result = text;
        }
    })
    if (Result == "Error") {
        ReturnErrorUrl();
    }

    return Result;
}


function getRootPath() {
    var strFullPath = window.document.location.href;
    var strPath = window.document.location.pathname;
    var pos = strFullPath.indexOf(strPath);
    var prePath = strFullPath.substring(0, pos);
    var postPath = strPath.substring(0, strPath.substr(1).indexOf('/') + 1);
    return (prePath + postPath);
}




//正则方法解析Url传值   例：alert(GetQueryString("参数名1"));
function GetQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}

//日期格式化日期
function GetDate(now, itype) {
    var year = now.getFullYear();
    var month = now.getMonth() + 1;
    var date = now.getDate();
    var hour = now.getHours();
    var minute = now.getMinutes();
    var second = now.getSeconds();
    if (itype == "yyyy-MM-dd hh:mm:ss")
        return year + "-" + month + "-" + date + "   " + hour + ":" + minute + ":" + second;
    else if (itype == "yyyy-MM-dd")
        return year + "-" + month + "-" + date;
    else if (itype == "yyyy-MM")
        return year + "-" + month;
    else if (itype == "MM-dd")
        return month + "-" + date;

}


function OpenForm(url, titleName, width, height) {

    var opener = art.dialog.top;
    opener.art.dialog.open(url, {
        title: titleName,
        lock: true,
        background: '#C6DEB9', // 背景色
        opacity: 0.87,
        resize: false,
        width: width,
        height: height,
        drag: true
    });
}

//是否IE浏览器
function isIE() {
    if (window.navigator.userAgent.toString().toLowerCase().indexOf("msie") >= 1)
        return true;
    else
        return false;
}

//清空界面，form为控件名称
function Clear(form) {
    $(':input', '#' + form)
     .not(':button, :submit, :reset, :hidden')
     .val('')
     .removeAttr('checked')
     .removeAttr('selected');

}

/***********************动态加载CSS，js*******************/
var dynamicLoading = {
    css: function (path) {
        if (!path || path.length === 0) {
            throw new Error('argument "path" is required !');
        }
        var head = document.getElementsByTagName('head')[0];
        var link = document.createElement('link');
        link.href = path;
        link.rel = 'stylesheet';
        link.type = 'text/css';
        head.appendChild(link);
    },
    js: function (path) {
        if (!path || path.length === 0) {
            throw new Error('argument "path" is required !');
        }
        var head = document.getElementsByTagName('head')[0];
        var script = document.createElement('script');
        script.src = path;
        script.type = 'text/javascript';
        head.appendChild(script);
    }
}

//动态加载 CSS 文件
dynamicLoading.css("mask.css");

//动态加载 JS 文件
//dynamicLoading.js("test.js");
/***********************遮罩层*******************/