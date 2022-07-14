// export  const rootUrl = "https://universalgames.buk.no/api/"
export const rootUrl = "https://10.0.0.3:3000/api/"
export function initData(url, data) {
  // const rootUrl = "/api/"

  const teamCode = myGetTeamCodeFunction()

  const response = fetch(rootUrl + url + teamCode, {
    method: "GET", // *GET, POST, PUT, DELETE, etc.
    headers: {
      "Accept": "application/json",
    },
    body: JSON.stringify(data), // body data type must match "Content-Type" header
  })

  return response; // parses JSON response into native JavaScript objects
}

export function getData(url, data) {
  // const rootUrl = "/api/"

  const teamCode = myGetTeamCodeFunction()

  const response = fetch(rootUrl + teamCode + url, {
    method: "GET", // *GET, POST, PUT, DELETE, etc.
    headers: {
      "Accept": "application/json",
    },
    body: JSON.stringify(data), // body data type must match "Content-Type" header
  })

  return response; // parses JSON response into native JavaScript objects
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
  // Get from local storage
  return window.localStorage.getItem('teamCode');
}