<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ClientSide.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        #placeDiv {
            position: relative;
            left: 25%;
            width: 50%;
            display: inline-flex;
            list-style: none;
        }

        #placeDiv li {
            margin: auto;
            width: 33%;
        }

        #placeDiv li:first-child {
            margin-left: 0;
        }

        #placeDiv li label {
            padding: 2%;
        }

        #psLbl {
            position: relative;
            margin-left: 9%;
        }

        #passengers {
            position: relative;
            width: 93%;
            display: inline-flex;
            list-style: none;
        }

        #passengers li {
            margin: auto;
            display: inline-flex;
            flex-direction: column;
            text-align: center;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Panel runat="server" ID="flightPanel">

        <ul id="placeDiv">
            <li>
                <div>
                    
                    <label>Flight<br />
                    From:</label>
                    <asp:DropDownList ID="From" AutoPostBack="True" runat="server">
                        <asp:ListItem Selected="True" Value="0"> Stockholm </asp:ListItem>
                        <asp:ListItem Value="1"> Copenhagen </asp:ListItem>
                        <asp:ListItem Value="2"> Paris </asp:ListItem>
                        <asp:ListItem Value="3"> London </asp:ListItem>
                        <asp:ListItem Value="4"> Frankfurt </asp:ListItem>
                    </asp:DropDownList>
                </div>
            </li>
            <li>
                <div>
                    <label>To:</label>
                    <asp:DropDownList ID="To"
                                      AutoPostBack="True"
                                      runat="server">
                        <asp:ListItem Value="0"> Stockholm </asp:ListItem>
                        <asp:ListItem Selected="True" Value="1"> Copenhagen </asp:ListItem>
                        <asp:ListItem Value="2"> Paris </asp:ListItem>
                        <asp:ListItem Value="3"> London </asp:ListItem>
                        <asp:ListItem Value="4"> Frankfurt </asp:ListItem>
                    </asp:DropDownList>
                </div>
            </li>

            <li>
                <label>Month:</label>
                <asp:DropDownList ID="MonthPick"
                                  AutoPostBack="True"
                                  runat="server">

                    <asp:ListItem Selected="True" Value="JAN"> January </asp:ListItem>
                    <asp:ListItem Value="FEB"> February </asp:ListItem>
                    <asp:ListItem Value="MAR"> Mars </asp:ListItem>
                    <asp:ListItem Value="APR"> April </asp:ListItem>
                    <asp:ListItem Value="MAY"> May </asp:ListItem>
                    <asp:ListItem Value="JUN"> June </asp:ListItem>
                    <asp:ListItem Value="JUL"> July </asp:ListItem>
                    <asp:ListItem Value="AUG"> August </asp:ListItem>
                    <asp:ListItem Value="SEP"> September </asp:ListItem>
                    <asp:ListItem Value="OCT"> October </asp:ListItem>
                    <asp:ListItem Value="NOV"> November </asp:ListItem>
                    <asp:ListItem Value="DEC"> December </asp:ListItem>

                </asp:DropDownList>
            </li>
        </ul>
        <div>
            <label id="psLbl">Number of Passengers:</label>
            <ul id="passengers">
                <li>
                    <label>Infants(<2)</label>
                    <asp:TextBox ID="infants" runat="server"></asp:TextBox>
                </li>
                <li>
                    <label>Children(<13)</label>
                    <asp:TextBox ID="children" runat="server"></asp:TextBox>
                </li>
                <li>
                    <label>Adults</label>
                    <asp:TextBox ID="adults" runat="server"></asp:TextBox>
                </li>
                <li>
                    <label>Seniors(>65)</label>
                    <asp:TextBox ID="seniorNbr" runat="server"></asp:TextBox>
                </li>
                <li>
                    Flightcompany:<asp:TextBox ID="flightCompany" runat="server"></asp:TextBox>
                </li>
                <li>FlightNumber:<asp:TextBox ID="flightNumber" runat="server" ></asp:TextBox>
                </li>
                <li>passNumber:
                    <asp:TextBox ID="passNumber" runat="server" ></asp:TextBox>
                </li>
                <li>DepartDate:
                    <asp:TextBox ID="departDate" runat="server" ></asp:TextBox>
                </li>
            </ul>
        </div>
            </asp:Panel>
        <br />
        <asp:Panel runat="server" ID="hotelPanel" Visible="false">
        Hotel<br />
        <br />
&nbsp;&nbsp;&nbsp; Room Type:<asp:RadioButtonList ID="hotelRoom" runat="server" ToolTip="Test">
    <asp:ListItem Selected ="True" Value="600"> Single Room </asp:ListItem>
    <asp:ListItem Value="900"> Double Room </asp:ListItem>
        </asp:RadioButtonList>
        <br />
&nbsp;&nbsp;&nbsp; Travellers:
        <asp:TextBox ID="travellers" runat="server"></asp:TextBox>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Nights:
        <asp:TextBox ID="nights" runat="server"></asp:TextBox>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Seniors:
        <asp:TextBox ID="seniors" runat="server"></asp:TextBox>
&nbsp;&nbsp;&nbsp; Primary Guest:
        <asp:TextBox ID="primGuest" runat="server"></asp:TextBox>
            &nbsp;&nbsp;&nbsp;&nbsp; passNumber:
            <asp:TextBox ID="hotelPassNumber" runat="server"></asp:TextBox>
            <br />
            <br />
            &nbsp;&nbsp;&nbsp; Arrival Date:&nbsp;
            <asp:TextBox ID="hotelDate" runat="server"></asp:TextBox>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:DropDownList ID="hotelList" runat="server" AutoPostBack="True">
                <asp:ListItem Selected="True" Value="1"> TheBrit </asp:ListItem>
                <asp:ListItem Value="2"> Spaghetti </asp:ListItem>
                <asp:ListItem Value="3"> Danske </asp:ListItem>
                <asp:ListItem Value="4"> Kungen </asp:ListItem>
                <asp:ListItem Value="5"> Revolution </asp:ListItem>
                <asp:ListItem Value="6"> Snapphanen </asp:ListItem>
            </asp:DropDownList>
        <br />
        <br />
            </asp:Panel>
        <br />
        <asp:Panel runat="server" ID="carPanel" Visible="false">
        Car<br />
        <br />
&nbsp;&nbsp;&nbsp; Fuel Type<asp:RadioButtonList ID="fuelType" runat="server" ToolTip="Test" >
    <asp:ListItem Selected ="True" Value="0"> Benzene </asp:ListItem>
    <asp:ListItem Value="1"> Diesel </asp:ListItem>
        </asp:RadioButtonList>
        <br />
        Seats: <asp:TextBox ID="seats" runat="server"></asp:TextBox>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Model Year:
        <asp:TextBox ID="modelYear" runat="server"></asp:TextBox>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Driver Age:
        <asp:TextBox ID="driverAge" runat="server"></asp:TextBox>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; carDate:
            <asp:TextBox ID="carDate" runat="server"></asp:TextBox>
            <br />
            <br />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; License date:
            <asp:TextBox ID="licensePlate" runat="server"></asp:TextBox>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; DriverLicense<asp:TextBox ID="driverLicense" runat="server"></asp:TextBox>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Rentalcompany(3chars)<asp:TextBox ID="rentalCompany" runat="server"></asp:TextBox>
            <br />
        <br />
            </asp:Panel>
        <asp:Panel runat="server" ID="paymentPanel" Visible="false">
        Payment<br />
        <p style="margin-left: 40px">Credit Card Number: <asp:TextBox ID="cardNbr" runat="server" Width="296px"></asp:TextBox>
        </p>
        <p style="margin-left: 40px">Card holder name:<asp:TextBox ID="cardHolder" runat="server"></asp:TextBox>
        </p>
        <p style="margin-left: 40px">
            Expire date:
            <asp:TextBox ID="expireDate" runat="server"></asp:TextBox>
            </p>
            <p style="margin-left: 40px">
                Balance:
                <asp:TextBox ID="balance" runat="server"></asp:TextBox>
            </p>
        <p style="margin-left: 40px">
            <asp:Button ID="BtnGet" runat="server" OnClick="BtnGet_Click" Text="Get Offer" />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                </p>
        <p>
            The total cost is:
            <asp:Label ID="totalCost" runat="server"></asp:Label>
            SEK </p>

            <p style="margin-left: 40px">
                &nbsp;</p>

        </asp:Panel>
        <p style="margin-left: 40px">
            <asp:Button ID="flightHide" runat="server" OnClick="flight_Click" Text="flight" />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="hotelHide" runat="server" OnClick="hotel_Click" Text="hotel" />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="carHide" runat="server" OnClick="car_Click" Text="car" />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="paymentHide" runat="server" OnClick="payment_Click" Text="payment" />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            </p>
        <p style="margin-left: 40px">
            &nbsp;
            <asp:SqlDataSource ID="listAirports" runat="server" ConnectionString="<%$ ConnectionStrings:testdbConnectionString %>" SelectCommand="SELECT [city] FROM [airports]"></asp:SqlDataSource>
            &nbsp;&nbsp;&nbsp;&nbsp;
            </p>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<div style="margin-left: 40px">
            <asp:SqlDataSource ID="listAirlines" runat="server" ConnectionString="<%$ ConnectionStrings:testdbConnectionString %>" SelectCommand="SELECT [name] FROM [airlines]"></asp:SqlDataSource>
        </div>
&nbsp;<div style="margin-left: 40px">
            <asp:SqlDataSource ID="listRoutesFrom" runat="server" ConnectionString="<%$ ConnectionStrings:testdbConnectionString %>" SelectCommand="SELECT * FROM [routes] WHERE departure='london' AND arrival='paris'" > </asp:SqlDataSource>
            
            <asp:SqlDataSource ID="listPriceandDeparture" runat="server" ConnectionString="<%$ ConnectionStrings:testdbConnectionString %>" SelectCommand="SELECT [price] FROM [flights] WHERE flightNbr='1' AND departureDate='1'" ></asp:SqlDataSource>
            
            <asp:SqlDataSource ID="customerInfo" runat="server" ConnectionString="<%$ ConnectionStrings:testdbConnectionString %>" SelectCommand="SELECT * FROM [customer]"></asp:SqlDataSource>
            
            <asp:SqlDataSource ID="transactions" runat="server" ConnectionString="<%$ ConnectionStrings:testdbConnectionString %>" SelectCommand="SELECT * FROM [transactions] WHERE ([cardNbr] = @cardNbr)">
                <SelectParameters>
                    <asp:Parameter DefaultValue="1234" Name="cardNbr" Type="Int32" />
                </SelectParameters>
            </asp:SqlDataSource>
        </div>

        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataSourceID="listAirports">
            <Columns>
                <asp:BoundField DataField="city" HeaderText="city" SortExpression="city" />
            </Columns>
        </asp:GridView>
        <br />
        <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" DataSourceID="listAirlines">
            <Columns>
                <asp:BoundField DataField="name" HeaderText="name" SortExpression="name" />
            </Columns>
        </asp:GridView>
        <br />
        <asp:GridView ID="GridView3" runat="server" DataSourceID="listRoutesFrom">
        </asp:GridView>
        <br />
        <asp:GridView ID="GridView4" runat="server" DataSourceID="listPriceandDeparture">
        </asp:GridView>

        <br />
        <asp:GridView ID="GridView5" runat="server" DataSourceID="customerInfo">
        </asp:GridView>
        <br />
        <asp:GridView ID="GridView6" runat="server" DataSourceID="transactions" AutoGenerateColumns="False" DataKeyNames="cardNbr">
            <Columns>
                <asp:BoundField DataField="cardNbr" HeaderText="cardNbr" ReadOnly="True" SortExpression="cardNbr" />
                <asp:BoundField DataField="date" HeaderText="date" SortExpression="date" />
                <asp:BoundField DataField="amount" HeaderText="amount" SortExpression="amount" />
            </Columns>
        </asp:GridView>
        
            <asp:GridView ID="transaction" runat="server" AutoGenerateColumns="True"></asp:GridView>
            <asp:GridView ID="customer" runat="server" AutoGenerateColumns="True"></asp:GridView>
        
    </form>
</body>
</html>
