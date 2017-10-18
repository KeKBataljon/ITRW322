using System.Collections.Generic;

namespace com.nqr.smsapp
{
	using Manifest = android.Manifest;
	using ContentResolver = android.content.ContentResolver;
	using PackageManager = android.content.pm.PackageManager;
	using Cursor = android.database.Cursor;
	using Uri = android.net.Uri;
	using NonNull = android.support.annotation.NonNull;
	using ContextCompat = android.support.v4.content.ContextCompat;
	using AppCompatActivity = android.support.v7.app.AppCompatActivity;
	using Bundle = android.os.Bundle;
	using SmsManager = android.telephony.SmsManager;
	using View = android.view.View;
	using ArrayAdapter = android.widget.ArrayAdapter;
	using EditText = android.widget.EditText;
	using ListView = android.widget.ListView;
	using Toast = android.widget.Toast;

	public class MainActivity : AppCompatActivity
	{

		internal List<string> smsMessagesList = new List<string>();
		internal ListView messages;
		internal ArrayAdapter arrayAdapter;
		internal EditText input;
		internal SmsManager smsManager = SmsManager.Default;
		private static MainActivity inst;

		private const int READ_SMS_PERMISSIONS_REQUEST = 1;

		public static MainActivity instance()
		{
			return inst;
		}

		public override void onStart()
		{
			base.onStart();
			inst = this;
		}


		protected internal override void onCreate(Bundle savedInstanceState)
		{
			base.onCreate(savedInstanceState);
			ContentView = R.layout.activity_main;
			messages = (ListView) findViewById(R.id.messages);
			input = (EditText) findViewById(R.id.input);
			arrayAdapter = new ArrayAdapter<>(this, android.R.layout.simple_list_item_1, smsMessagesList);
			messages.Adapter = arrayAdapter;
			if (ContextCompat.checkSelfPermission(this, Manifest.permission.READ_SMS) != PackageManager.PERMISSION_GRANTED)
			{
				PermissionToReadSMS;
			}
			else
			{
				refreshSmsInbox();
			}


		}


		public virtual void updateInbox(string smsMessage)
		{
			arrayAdapter.insert(smsMessage, 0);
			arrayAdapter.notifyDataSetChanged();
		}

	public virtual void onSendClick(View view)
	{

		if (ContextCompat.checkSelfPermission(this, Manifest.permission.SEND_SMS) != PackageManager.PERMISSION_GRANTED)
		{
			PermissionToReadSMS;
		}
		else
		{
			smsManager.sendTextMessage("07701056337", null, input.Text.ToString(), null, null);
			Toast.makeText(this, "Message sent!", Toast.LENGTH_SHORT).show();
		}
	}

			public virtual void getPermissionToReadSMS()
			{
				if (ContextCompat.checkSelfPermission(this, Manifest.permission.READ_SMS) != PackageManager.PERMISSION_GRANTED)
				{
					if (shouldShowRequestPermissionRationale(Manifest.permission.READ_SMS))
					{
						Toast.makeText(this, "Please allow permission!", Toast.LENGTH_SHORT).show();
					}
					requestPermissions(new string[]{Manifest.permission.READ_SMS}, READ_SMS_PERMISSIONS_REQUEST);
				}
			}


		public override void onRequestPermissionsResult(int requestCode, string[] permissions, int[] grantResults)
		{
			// Make sure it's our original READ_CONTACTS request
			if (requestCode == READ_SMS_PERMISSIONS_REQUEST)
			{
				if (grantResults.Length == 1 && grantResults[0] == PackageManager.PERMISSION_GRANTED)
				{
					Toast.makeText(this, "Read SMS permission granted", Toast.LENGTH_SHORT).show();
					refreshSmsInbox();
				}
				else
				{
						 Toast.makeText(this, "Read SMS permission denied", Toast.LENGTH_SHORT).show();
				}

			}
				else
				{
					base.onRequestPermissionsResult(requestCode, permissions, grantResults);
				}



		}

				public virtual void refreshSmsInbox()
				{
				ContentResolver contentResolver = ContentResolver;
				Cursor smsInboxCursor = contentResolver.query(Uri.parse("content://sms/inbox"), null, null, null, null);
				int indexBody = smsInboxCursor.getColumnIndex("body");
				int indexAddress = smsInboxCursor.getColumnIndex("address");
				if (indexBody < 0 || !smsInboxCursor.moveToFirst())
				{
					return;
				}
				arrayAdapter.clear();
				do
				{
					string str = "SMS From: " + smsInboxCursor.getString(indexAddress) +
							"\n" + smsInboxCursor.getString(indexBody) + "\n";
					arrayAdapter.add(str);
				} while (smsInboxCursor.moveToNext());
	//messages.setSelection(arrayAdapter.getCount() - 1);
				}



	}

}