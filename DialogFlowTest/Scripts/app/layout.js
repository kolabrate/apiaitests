/**
 * Copyright 2017 Google Inc. All Rights Reserved.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
 
(function() {
  "use strict";

  var ENTER_KEY_CODE = 13;
  var queryInput, resultDiv, accessTokenInput, numberInUseInput;

  window.onload = init;

  function init() {
    queryInput = document.getElementById("q");
    resultDiv = document.getElementById("result");
    accessTokenInput = document.getElementById("access_token");
    numberInUseInput = document.getElementById("numberInUse");
    var setAccessTokenButton = document.getElementById("set_access_token");

    var getNewNumberButton = document.getElementById("getNewNumber");

    queryInput.addEventListener("keydown", queryInputKeyDown);
    //setAccessTokenButton.addEventListener("click", setAccessToken);
    getNewNumberButton.addEventListener("click", getNewNumber);
    setAccessToken();
  }

  function getNewNumber() {
      setAccessToken();
      var newNumber = "-61" + new Date().getTime();
      numberInUseInput.value = newNumber;
      resultDiv.innerHTML = '';
  }

  function setAccessToken() {
    //document.getElementById("placeholder").style.display = "none";
    //document.getElementById("main-wrapper").style.display = "block";
    //window.init(accessTokenInput.value);
    window.init("3e6c9aff32be480aabf56bfd796929e5");
  }

  function queryInputKeyDown(event) {
      if (event.which !== ENTER_KEY_CODE) {
          return;
      }

      var value = queryInput.value;
      queryInput.value = "";

      createQueryNode(value);
      var responseNode = createResponseNode();

      var fromNum = numberInUseInput.value;

      //sendText(value, fromNum)
      //    .then(function (response) {
      //        setResponseJSON(response);
      //        setResponseOnNode(response.result.fulfillment.speech, responseNode);

      //        if (response.result.fulfillment.displayText == "Checking" || response.result.fulfillment.displayText == "CheckingConfirmation") {                  
      //            responseNode = createResponseNode();
      //            var fromNum = numberInUseInput.value;
      //            $.ajax({
      //                type: 'GET',
      //                url: '/api/Messages',
      //                data: jQuery.param({ value: fromNum }),
      //                success: function (msg) {
      //                    setSecondResponseOnNode(msg, responseNode);
      //                },
      //                error: function (msg) {
      //                    console.log(JSON.stringify(msg));                          
      //                    setSecondResponseOnNode(JSON.stringify(msg), responseNode);
      //                }
      //            });
      //        }
      //    })
      //    .catch(function (err) {
      //        setResponseJSON(err);
      //        setResponseOnNode("Something goes wrong", responseNode);
      //    });

      $.ajax({
          type: 'GET',
          url: '/api/Messages/SendTextAsync',
          data: jQuery.param({ value: value, number: fromNum }),
          success: function (msg) {
              //setSecondResponseOnNode(msg, responseNode);
              return msg;
          },
          error: function (msg) {
              //console.log(JSON.stringify(msg));
              //setSecondResponseOnNode(JSON.stringify(msg), responseNode);
              return msg;
          }
      });
  }

  function createQueryNode(query) {
    var node = document.createElement('div');
    node.className = "clearfix left-align left card-panel green accent-1";
    node.innerHTML = query;
    //resultDiv.appendChild(node);
    resultDiv.insertBefore(node, resultDiv.firstChild);
  }

  function createResponseNode() {
    var node = document.createElement('div');
    node.className = "clearfix right-align right card-panel blue-text text-darken-2 hoverable";
    node.innerHTML = "...";
    //resultDiv.appendChild(node);
    resultDiv.insertBefore(node, resultDiv.firstChild);
    return node;
  }

  function setResponseOnNode(response, node) {
    node.innerHTML = response ? response : "[empty response]";
    node.setAttribute('data-actual-response', response);
  }

  function setSecondResponseOnNode(response, node) {
      node.innerHTML = node.innerHTML+ response;
  }

  function setResponseJSON(response) {
    //var node = document.getElementById("jsonResponse");
    //node.innerHTML = JSON.stringify(response, null, 2);
  }

  function sendRequest() {

  }

})();
