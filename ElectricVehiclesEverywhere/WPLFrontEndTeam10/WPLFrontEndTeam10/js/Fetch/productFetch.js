let productenArray = [];
let uniqueProductenArray=[];


function  getProducten()
    {
        return fetch(apiUrl+'product')
                .then(validateResponse)
                .then(readResponseAsJSON)
                .then(data => { productenArray=data;
                                uniqueProductenArray = [
                                    ...new Map(productenArray.map((item) => [item["naam"], item])).values(),
                                ];
                                return uniqueProductenArray;
                                console.log(uniqueProductenArray);
                })
                .catch(logError);

    }

window.onload = getProducten();






