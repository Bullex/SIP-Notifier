using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIP_Notifier
{
    class HistoryRow
    {
        private int mId;
        private string mText;
        private string mPhone;
        private string mDate;

        public HistoryRow(int id, string text, string phone, string date)
        {
            mId = id;
            mText = text;
            mPhone = phone;
            mDate = date;
        }

        public int getId()
        {
            return mId;
        }

        public string getText()
        {
            return mText;
        }

        public string getDate()
        {
            return mDate;
        }

        public string getPhone()
        {
            return mPhone;
        }

    }
}
