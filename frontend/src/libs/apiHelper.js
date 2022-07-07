export function getData(url, data) {
    const rootUrl = "https://universalgames.buk.no/api/"
    // const rootUrl = "/api/"

    const teamCode = myGetTeamCodeFunction()

    const response = fetch(rootUrl + teamCode + url, {
      method: "GET", // *GET, POST, PUT, DELETE, etc.
      headers: {
        Accept: "application/json",
        "Content-Type": "application/json",
      },
      body: JSON.stringify(data), // body data type must match "Content-Type" header
    })
  
    return response; // parses JSON response into native JavaScript objects
}

function myGetTeamCodeFunction() {
    // Get from local storage

    return "6EZ1FOV"
}