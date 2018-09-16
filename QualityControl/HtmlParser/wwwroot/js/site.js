$(document).ready(function () {
    addSearchHandler();
});

function addSearchHandler() {
    const LINKS_URL = '/api/links';
    const SEARCH_RESULT_PAGE = '/';

    input = $("#jq-input");
    button = $("#jq-button");
    normalLinksWrapper = $('#jq-normal-links-wrapper');

    button.click(function () {
        normalLinksWrapper.empty();
        url = input.val();
        $.post(LINKS_URL, { href: url })
            .done(function (data) {
                addItems(data, normalLinksWrapper);
            })
            .fail(function (data) {
                console.log(111);
                normalLinksWrapper.append("<p>Не удалось загрузить данные</p>");
            })
        return false;
    });
}

function addItems(data, itemsWrapper) {
    $.each(data, function (key, value) {
        link = '<a class="parsed-link"  target="_blank" href=\"' + value + '\">' + value + '</a>';
        itemsWrapper.append(link);
    });
}