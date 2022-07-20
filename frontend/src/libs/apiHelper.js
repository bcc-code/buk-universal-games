export const rootUrl = "https://universalgames.buk.no/api/"

export function initData(url, data) {
  const teamCode = myGetTeamCodeFunction()

  return fetch(rootUrl + url + teamCode, {
    method: "GET",
    headers: {
      "Accept": "application/json",
    },
    body: JSON.stringify(data),
  })
}

export function getData(url, data) {
  const teamCode = myGetTeamCodeFunction()

  return fetch(rootUrl + teamCode + url, {
    method: "GET",
    headers: {
      "Accept": "application/json",
    },
    body: JSON.stringify(data),
  })
}

export function postData(url) {
  const teamCode = myGetTeamCodeFunction()
  return fetch(rootUrl + teamCode + url, {
    method: "POST",
    headers: {
      Accept: "application/json",
    },
  })
    .then((r) => r.json())
    .then((r) => {
      console.log("POST data response OK", r);
      return r
    })
    .catch((e) => {
      console.log("POST data response ERROR", e);
      return e
    });
}

export function postStickerCode(stickerCode) {
  const teamCode = myGetTeamCodeFunction()
  return fetch(`${rootUrl}${teamCode}/stickers/${stickerCode}/scan`, {
    method: "POST",
    headers: {
      Accept: "application/json",
    },
  })
    .then((r) => r.json())
    .then((r) => {
      console.log("Sticker response OK", r);
      return r
    })
    .catch((e) => {
      console.log("Sticker response ERROR", e);
      return e
    });
}

export function myGetTeamCodeFunction() {
  return window.localStorage.getItem('teamCode');
}