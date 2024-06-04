import { rootUrl } from '../hooks/hooks';

const retryRequestAmount = 5; // Retry the request x times
const secondsBetweenRetry = 2; // Wait x seconds between each request

export function initData(store, url, data) {
  const teamCode = myGetTeamCodeFunction();
  let intervalRunning = false;
  store.commit('resetRequestRetry', url);

  return new Promise((resolve, reject) => {
    intervalFunction(resolve, reject);
    let currentResponse = null;
    const requestInterval = setInterval(
      () => intervalFunction(resolve, reject),
      secondsBetweenRetry * 1000,
    );

    setTimeout(
      () => {
        if (intervalRunning) {
          clearInterval(requestInterval);
          resolve(currentResponse);
        }
      },
      (secondsBetweenRetry + 1) * retryRequestAmount * 1000,
    );

    function intervalFunction(resolve, reject) {
      intervalRunning = true;
      store.commit('addRequestRetry', url);

      if (
        store.state.currentAmountOfRequestRetries[url] &&
        store.state.currentAmountOfRequestRetries[url] <= retryRequestAmount
      ) {
        sendRequest()
          .then((r) => {
            if (r.error) {
              currentResponse = r;
              if (r.errorCode === 403) {
                intervalRunning = false;
                currentResponse = r;
                clearInterval(requestInterval);
                resolve(currentResponse);
              }
            } else {
              intervalRunning = false;
              currentResponse = r;
              clearInterval(requestInterval);
              resolve(currentResponse);
            }
          })
          .catch((e) => {
            console.error(e);
            reject(e);
          });
      } else {
        intervalRunning = false;
        clearInterval(requestInterval);
        resolve(currentResponse);
      }
    }

    function sendRequest() {
      let requestStatusCode = 0;
      return fetch(rootUrl + url + teamCode, {
        method: 'GET',
        headers: {
          Accept: 'application/json',
        },
        body: JSON.stringify(data),
      })
        .then((r) => {
          requestStatusCode = r.status;

          return r.json();
        })
        .then((r) => {
          if (requestStatusCode !== 200) {
            throw r;
          }

          return r;
        })
        .catch((e) => {
          if (e.error) {
            return { ...e, errorCode: requestStatusCode };
          }

          return { error: e, errorCode: requestStatusCode };
        });
    }
  });
}

export function getData(store, url, data) {
  const teamCode = myGetTeamCodeFunction();
  let intervalRunning = false;
  store.commit('resetRequestRetry', url);

  return new Promise((resolve, reject) => {
    intervalFunction(resolve, reject);
    let currentResponse = null;
    const requestInterval = setInterval(
      () => intervalFunction(resolve, reject),
      secondsBetweenRetry * 1000,
    );

    setTimeout(
      () => {
        if (intervalRunning) {
          clearInterval(requestInterval);
          resolve(currentResponse);
        }
      },
      (secondsBetweenRetry + 1) * retryRequestAmount * 1000,
    );

    function intervalFunction(resolve, reject) {
      intervalRunning = true;
      store.commit('addRequestRetry', url);

      if (
        store.state.currentAmountOfRequestRetries[url] &&
        store.state.currentAmountOfRequestRetries[url] <= retryRequestAmount
      ) {
        sendRequest()
          .then((r) => {
            if (r.error && r.errorCode != 406) {
              currentResponse = r;
            } else {
              intervalRunning = false;
              currentResponse = r;
              clearInterval(requestInterval);
              resolve(currentResponse);
            }
          })
          .catch((e) => {
            console.error(e);
            reject(e);
          });
      } else {
        intervalRunning = false;
        clearInterval(requestInterval);
        resolve(currentResponse);
      }
    }

    function sendRequest() {
      let requestStatusCode = 0;
      return fetch(rootUrl + url, {
        method: 'GET',
        headers: {
          Accept: 'application/json',
          'x-ubg-teamcode': teamCode,
        },
        body: JSON.stringify(data),
      })
        .then((r) => {
          requestStatusCode = r.status;

          return r.json();
        })
        .then((r) => {
          if (requestStatusCode !== 200) {
            throw r;
          }

          return r;
        })
        .catch((e) => {
          if (e.error) {
            return { ...e, errorCode: requestStatusCode };
          }

          return { error: e, errorCode: requestStatusCode };
        });
    }
  });
}

export async function postData(url, data) {
  const teamCode = myGetTeamCodeFunction();
  try {
    const r = await fetch(rootUrl + url, {
      method: 'POST',
      headers: {
        Accept: 'application/json',
        'Content-Type': 'application/json',
        'x-ubg-teamcode': teamCode,
      },
      body: JSON.stringify(data),
    });

    if (r.ok) {
      if (r.bodyUsed && r.body.startsWith('{')) {
        return JSON.parse(r.body);
      }
      return r.body;
    }
    return {
      error: r.statusText,
      response: r,
    };
  } catch (e) {
    console.log('POST data response ERROR', e);
    throw e;
  }
}

export function myGetTeamCodeFunction() {
  const teamCode = window.localStorage.getItem('testTeamCode');
  if (!teamCode) throw Error('team code is not defined');
  return teamCode;
}
