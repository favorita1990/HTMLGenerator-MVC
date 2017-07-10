/* open and close sidenav */
function myFunction() {
    var x = document.getElementById("side");
    if (x.className === "sidenav") {
        x.className += " hide-show";
    } else {
        x.className = "sidenav";
    }
}
///* copy btn */
//$("button").click(function () {
//    $("#generatedCode").select();
//    document.execCommand('copy');
//    alert("The Code was Copied!");
//});
/* table gen */
function generateTable() {
    rowcount = document.CodeGenerator.RowCount.value;
    columncount = document.CodeGenerator.ColumnCount.value;
    displaysampletext = document.CodeGenerator.DisplaySampleText.value;
    borderstyle = document.CodeGenerator.BorderStyle.value;

    tablerows = '<tbody>\n';
    tablehead = '<thead>\n<tr>\n';
    tableheadtext = '';
    tabletext = '<td></td>\n';

    for (thisRow = 1; thisRow <= rowcount; thisRow++) {

        tablerows = tablerows + '<tr>\n';

        for (thisColumn = 1; thisColumn <= columncount; thisColumn++) {

            /* Header */
            if (thisRow == 1) {
                if (displaysampletext == "Yes") {
                    tableheadtext = tableheadtext + '<th>Header Cell ' + thisColumn + '</th>\n';
                } else {
                    tableheadtext = tableheadtext + '<th></th>\n';
                }
            }

            /* Normal Row */
            if (displaysampletext == "Yes") {

                tabletext = "<td>Row " + thisRow + ", Cell " + thisColumn + "</td>\n";

            }

            tablerows = tablerows + tabletext;
        }
        tablerows = tablerows + '</tr>\n';
    }

    tablehead = tablehead + tableheadtext + '</tr>\n</thead>\n';
    tablerows = tablerows + '</tbody>\n';

    output =

        '<style>\n' +


        'table.GeneratedTable {\n' +
        ((borderstyle) ? 'border-style:' + borderstyle + ';\n' : '') +
        '}\n\n' +

        'table.GeneratedTable td, table.GeneratedTable th {\n' +
        ((borderstyle) ? 'border-style:' + borderstyle + ';\n' : '') +
        '}\n\n' +

        '</style>\n\n' +

        '<table border="2" class="GeneratedTable">\n' +

        tablehead +
        tablerows +
        '</table>\n';


    document.CodeGenerator.generatedCode.value = output;
    document.getElementById('displayResult').innerHTML = output;

    return output;
}
/* */
/* button gen */
function generateButton() {
    pickbuttontype = document.ButtonGenerator.PickButtonType.value;
    buttontext = document.ButtonGenerator.ButtonText.value;
    buttonname = document.ButtonGenerator.ButtonName.value;
    buttontarget = document.ButtonGenerator.ButtonTarget.value;

    output =

        ((pickbuttontype) ? '<button type="' +
            pickbuttontype + '" name="' +
            buttonname + '"' + ' formtarget="' +
            buttontarget + '">' + buttontext + '</button>' : '')
    '\n';

    document.ButtonGenerator.generatedCode.value = output;
    document.getElementById('displayResult').innerHTML = output;

    return output;
}

/* */
/* linksGen */
function generateLink() {
    linktext = document.LinkGenerator.LinkText.value;
    linkhref = document.LinkGenerator.LinkHref.value;
    linktarget = document.LinkGenerator.linkTarget.value;

    output =

        ((linktext) ? '<a href="' + linkhref + '" target="' + linktarget + '">' : '') +
        ((linktext) ? linktext + '</a>' : '') +
        '\n';

    document.LinkGenerator.generatedCode.value = output;
    document.getElementById('displayResult').innerHTML = output;

    return output;
}

/* */
/* heading gen */
function generateHeading() {
    headingtext = document.HeadingGenerator.HeadingText.value;
    heading = document.HeadingGenerator.Heading.value;

    output =

        ((heading) ? '<' + heading + '>' +
            headingtext + '</' + heading + '>' : '')
    '\n';

    document.HeadingGenerator.generatedCode.value = output;
    document.getElementById('displayResult').innerHTML = output;

    return output;
}
/* */
/* paragraph gen */
function generateParagraph() {
    paragraphtext = document.ParagraphGenerator.ParagraphText.value;
    paragraphstyle = document.ParagraphGenerator.ParagraphStyle.value;

    output =

        ((paragraphstyle) ? '<' + paragraphstyle + '>' +
            paragraphtext + '</' + paragraphstyle + '>' : '')
    '\n';


    document.ParagraphGenerator.generatedCode.value = output;
    document.getElementById('displayResult').innerHTML = output;

    return output;
}
/* */
/* formattingGen */
function generateFormat() {
    formattext = document.FormattingGen.FormatText.value;
    format = document.FormattingGen.Format.value;

    output =

        ((format) ? '<' + format + '>' : '') +
        ((formattext) ? formattext + '</' : '') + format + '>';
    '\n';

    document.FormattingGen.generatedCode.value = output;
    document.getElementById('displayResult').innerHTML = output;

    return output;
}

/* */
/* image gen */
function generateImage() {
    imagesource = document.ImageGenerator.ImageSource.value;
    imagealt = document.ImageGenerator.ImageAlt.value;
    imagewidth = document.ImageGenerator.ImageWidth.value;
    imageheight = document.ImageGenerator.ImageHeight.value;

    output =

        ((imagesource) ? '<img src="' + imagesource + '" alt="' +
            imagealt + '" width="' + imagewidth + '" height="' + imageheight + '">' : '')
    '\n';

    document.ImageGenerator.generatedCode.value = output;
    document.getElementById('displayResult').innerHTML = output;

    return output;

}
/* */
/* Lists Gen */
function generateList() {
    listtext = document.ListsGenerator.ListText.value;
    ulstyletype = document.ListsGenerator.UlstyleType.value;

    output =

        ((listtext) ? '<ul style="list-style-type:' + ulstyletype + '">\n   <li>' +
            listtext + '</li>\n   <li>' + listtext + '</li>\n   <li>' +
            listtext + '</li>\n</ul>' : '')
    '\n';

    document.ListsGenerator.generatedCode.value = output;
    document.getElementById('displayResult').innerHTML = output;

    return output;
}
/* */
function generateList1() {
    listtext = document.ListsGenerator.ListText.value;
    olstyletype = document.ListsGenerator.OlstyleType.value;

    output =

        ((listtext) ? '<ol type="' + olstyletype + '">\n   <li>' +
            listtext + '</li>\n   <li>' + listtext + '</li>\n   <li>' +
            listtext + '</li>\n</ol>' : '')
    '\n';

    document.ListsGenerator.generatedCode.value = output;
    document.getElementById('displayResult').innerHTML = output;

    return output;
}
/* */
/* style gen */
function generateStyle() {
    bodycolor = document.StylesGenerator.PickBodyColor.value;
    bodytextsize = document.StylesGenerator.PickBodyTextSize.value;
    headingtext = document.StylesGenerator.HeadingText.value;
    headingtextcolor = document.StylesGenerator.HeadingTextColor.value;
    paragraphtext = document.StylesGenerator.ParagraphText.value;
    paragraphtextcolor = document.StylesGenerator.ParagraphTextColor.value;

    output =

        ((bodycolor) ? '<div style="' + "height:170px;" + bodycolor + bodytextsize + '">\n   <span style="' +
            headingtextcolor + '">' +
            headingtext + '</span>\n   <p style="' + paragraphtextcolor + '">' +
            paragraphtext + '</p>\n</div>' : '')
    '\n';

    document.StylesGenerator.generatedCode.value = output;
    document.getElementById('displayResult').innerHTML = output;

    return output;
}
/* */
/* header gen */
function generateHeader() {
    headertext = document.HeaderGenerator.HeaderText.value;
    pickheadercolor = document.HeaderGenerator.PickHeaderColor.value;

    output =
        '<style>\n' +
        '#GeneratedHeader {\n' +
        'padding: 15px;' +
        '\n}\n' +
        '#GeneratedHeader h1 {\n' +
        'text-align: center;' +
        'padding: 15px' +
        '\n}\n' +
        '</style>\n' +

        ((headertext) ? '<header id="GeneratedHeader" style="' +
            pickheadercolor + '">\n   <h1 style="' +
            headertext + '">' +
            headertext + '</h1>\n' + '</header>' : '')
    '\n';

    document.HeaderGenerator.generatedCode.value = output;
    document.getElementById('displayResult').innerHTML = output;

    return output;
}
/* */
/* nav gen */
function generateNav() {
    navtext = document.NavGenerator.NavText.value;
    picknavcolor = document.NavGenerator.PickNavColor.value;
    picknavbackhovercolor = document.NavGenerator.PickNavHoverColor.value;

    output =
        '<style>\n' +
        '#GeneratedNav {\n' +
        'background-color:red ;' +
        '\n}\n' +
        '#GeneratedNav ul {\n' +
        'list-style-type: none;' +
        'padding: 0;' +
        'margin: 0;' +
        'overflow: hidden;' +
        '' + picknavcolor + ';' +
        '\n}\n' +
        '#GeneratedNav ul li{\n' +
        'width: 33.33%;' +
        'float: left;' +
        '\n}\n' +
        '#GeneratedNav ul li a{\n' +
        'display: block;' +
        'color: white;' +
        'width: 100%;' +
        'text-align: center;' +
        'padding: 14px 16px;' +
        'text-decoration: none;' +
        '\n}\n' +
        '#GeneratedNav ul li a:hover{\n' +
        '' + picknavbackhovercolor + ';' +
        '\n}\n' +
        '</style>\n' +


        ((navtext) ? '<nav id="GeneratedNav" style="' +
            'color:white;">\n<ul>\n' +
            '   <li><a href="#">Home</a></li>\n' +
            '   <li><a href="#">About</a></li>\n' +
            '   <li><a href="#">Contact</a></li>' +
            '\n</ul>\n' + '</nav>' : '')
    '\n';

    document.NavGenerator.generatedCode.value = output;
    document.getElementById('displayResult').innerHTML = output;

    return output;
}
/* */
/* footer gen */
function generateFooter() {
    footertext = document.FooterGenerator.FooterText.value;
    footercolor = document.FooterGenerator.FooterColor.value;

    output =
        '<style>\n' +
        '#GeneratedFooter {\n' +
        '\n}\n' +
        '#GeneratedFooter p {\n' +
        'text-align: center;' +
        'color: white;' +
        'padding: 5px' +
        '\n}\n' +
        '</style>\n' +

        ((footertext) ? '<footer id="GeneratedFooter" style="' +
            footercolor + '">\n   <p style="' +
            footertext + '">' +
            footertext + '</p>\n' + '</footer>' : '')
    '\n';

    document.FooterGenerator.generatedCode.value = output;
    document.getElementById('displayResult').innerHTML = output;

    return output;
}
/* */
/* main gen */
function generateMain() {
    sectiontext = document.MainGenerator.SectionText.value;
    articletext = document.MainGenerator.ArticleText.value;
    asidetext = document.MainGenerator.AsideText.value;

    output =
        '<style>\n' +
        '#GeneratedMain p{\n' +
        'color: black;' +
        '\n}\n' +
        '#GeneratedMain section {\n' +
        'border: 1px solid black;' +
        'background-color: white;' +
        'width:70%;' +
        'float: left;' +
        'height: 145px;' +
        '\n}\n' +
        '#GeneratedMain article{\n' +
        'border: 1px solid black;' +

        'background-color: white;' +
        'width:70%;' +
        'float: left;' +
        'height: 145px;' +
        '\n}\n' +
        '#GeneratedMain aside{\n' +
        'border: 1px solid black;' +
        'background-color: white;' +
        'height: 290px;' +
        'width:30%;' +
        'float: right;' +
        '\n}\n' +
        '</style>\n' +


        ((sectiontext) ? '<main id="GeneratedMain" style="' +
            'color:white;">\n<aside>\n' +
            '   <p>' + asidetext + '</p>\n</aside>' +
            '\n<section>\n   <p>' + sectiontext + '</p>\n</section>' +
            '\n<article>\n   <p>' + articletext + '</p>\n</article>' +
            '\n' + '</main>' : '')
    '\n';

    document.MainGenerator.generatedCode.value = output;
    document.getElementById('displayResult').innerHTML = output;

    return output;
}
/* */