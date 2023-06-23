function validateEmail(email) {
    var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
    return re.test(String(email).toLowerCase())
}

function isNullOrEmpty(str) {
    return str == "" || str == null || str == undefined
}

function ajaxApi(axiosObj, callback, catchError = true) {
    let ajax = axios(axiosObj).then(response => {
        callback(response)
    })

    if (catchError) {
        return ajax.catch(error => {
            Swal.fire({
                text: '發生錯誤，請聯絡系統管理員，或稍後再試！',
                icon: 'error',
            })
        })
    }
    else {
        return ajax
    }
}