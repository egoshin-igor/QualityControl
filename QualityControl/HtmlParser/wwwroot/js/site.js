$(document).ready(function () {
    addSearchHandler();
});

function addSearchHandler() {
    const LINKS_URL = '/api/links';
    const SEARCH_RESULT_PAGE = '/';

    input = $("#jq-input");
    button = $("#jq-button");
    normalLinksWrapper = $('#jq-normal-links-wrapper');
    brokenLinksWrapper = $('#jq-broken-links-wrapper');
    errorBlock = $('#jq-error-block');

    button.click(function () {
        normalLinksWrapper.empty();
        brokenLinksWrapper.empty();
        errorBlock.empty();
        url = input.val();
        $.post(LINKS_URL, { href: url })
            .done(function (data) {
                addItems(data.normalLinks, normalLinksWrapper);
                addItems(data.brokenLinks, brokenLinksWrapper);
                console.log("normal:" + data.normalLinks.length);
                console.log("broken:" + data.brokenLinks.length);
            })
            .fail(function (data) {
                errorBlock.append(data);
                errorBlock.append("<p>Не удалось загрузить данные</p>");
            })
        return false;
    });
}

function addItems(data, itemsWrapper) {
    $.each(data, function (key, value) {
        link = '<a title=\"' + value.link + '\" class="parsed-link"  target="_blank" href=\"' + value.link + '\">' + value.link + '</a>';
        status = '<p class="status">Статус: ' + value.status + '</p>';
        div = '<div class=\"link-result\">' + link + status + '</div>';
        itemsWrapper.append(div);
    });
}