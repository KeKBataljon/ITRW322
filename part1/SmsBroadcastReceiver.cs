namespace com.nqr.smsapp
{

			using BroadcastReceiver = android.content.BroadcastReceiver;
			using Context = android.content.Context;
			using Intent = android.content.Intent;
			using Bundle = android.os.Bundle;
			using SmsMessage = android.telephony.SmsMessage;

	public class SmsBroadcastReceiver : BroadcastReceiver
	{

		public const string SMS_BUNDLE = "pdus";

		public virtual void onReceive(Context context, Intent intent)
		{
			Bundle intentExtras = intent.Extras;

				if (intentExtras != null)
				{
				object[] sms = (object[]) intentExtras.get(SMS_BUNDLE);
				string smsMessageStr = "";
				for (int i = 0; i < sms.Length; ++i)
				{
					string format = intentExtras.getString("format");
					SmsMessage smsMessage = SmsMessage.createFromPdu((sbyte[]) sms[i], format);

					string smsBody = smsMessage.MessageBody.ToString();
					string address = smsMessage.OriginatingAddress;

					smsMessageStr += "SMS From: " + address + "\n";
					smsMessageStr += smsBody + "\n";
				}

				MainActivity inst = MainActivity.instance();
				inst.updateInbox(smsMessageStr);
				}
		}
	}
}