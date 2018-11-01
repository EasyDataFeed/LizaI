using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            infoTextBlock.Text = @"There are 4 tabs 'Parse Users' 'Invite Bots' 'Invite Users' 'Log'  and the field for attaching the 'profiles file' on the interface.
===========================================
Profiles File: - choose the path to the file with the profiles with the 'Browse' button.
File you using should have .csv comma delimited format.
File should contain those columns:
PhoneNumber - user's phone number   * 380950123456
GroupName  - the name for logical separating of the users which will invite another users   * Monday
Proxy Ip - ip proxy which will be used for log in of current user * zproxy.luminati.io
Proxy Port - * 22225
Proxy Login -  * lum-customer-shopping_cart_elite-zone-user14-country-us-session-abctsefgs
Proxy Password - * 1a2b3c4d5h
===========================================
Parse Users:

Parsing the accounts information and saving results to the file.
--------------------------------

Press 'Save as' to choose the directory where the file with the scraped information about the users will be saved. 

The file will contain those fields: 'UserId' 'ChatId' 'ChatName' 'UserName' 'LastOnLine' 'LastMessage1' 'LastMessage2' 'LastMessage3'

Start - launching the scraping process 
===========================================
Invite Profiles: 

Let's accounts from Profiles File to join the chat/group which is specified on the link
--------------------------------

The field should contain the link to the group where users from Main File should be joined.

Invite - launching invite process
===========================================
Invite Users: 

Use this tab to get the users joined to the chats/groups you need. Invite will be generated from Profiles File's accounts.
Profiles File's accounts should be participants of the group that is specified on 'Group Link' or the should  be invited on invite profiles tab first.
--------------------------------

File with users id's - file with users which will be invited into the group
Required column - 'UserId' 
File should have .csv (comma delimited) format. 

Paste link to the group where users from file (File with users id's) should be invited into 'Group Link' 

Grid: 
Group Name - Main File group
Account in Group - number of accounts from Main File defined for current group
checkBox - use selected account for inviting the users

Amount of invites per acc per day -  total number of accounts invited by the group which is checked by the checkbox into the group on the link 

Delay between invitations (hours) - delay between each invitation. 

Invite - starting the invite process";
        }
    }
}
