const rootUrl = "https://universalgames.buk.no/api/"
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

function myGetTeamCodeFunction() {
    // Get from local storage

    console.log("window.localStorage.getItem('teamCode');", window.localStorage.getItem('teamCode'))

    return window.localStorage.getItem('teamCode');
}