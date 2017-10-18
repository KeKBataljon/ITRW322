using System;
using android.content.ContentResolver;
using android.content.Context;
using android.content.Intent;
using android.content.pm.PackageManager;
using android.database.Cursor;
using android.Manifest;
using android.net.Uri;
using android.os.Bundle;
using android.provider.ContactsContract;
using android.support.annotation.NonNull;
using android.support.v4.content.ContextCompat;
using android.support.v7.app.AppCompatActivity;
using android.telephony.SmsManager;
using android.view.View;
using android.widget.ArrayAdapter;
using android.widget.EditText;
using android.widget.ListView;
using android.widget.Toast;
using System.Collections.Generic;
using System.Collections;
	
	namespace com.nqr.smsapp
{

	public class MainActivity : AppCompatActivity
	{

		ArrayList<string> smsMessagesList = new ArrayList<>();
    ListView messages;
    ArrayAdapter arrayAdapter;
    EditText input;
    SmsManager smsManager = SmsManager.getDefault();
    private static MainActivity inst;
    public static bool active = false;

    private static readonly int READ_SMS_PERMISSIONS_REQUEST = 1;
    private static readonly int READ_CONTACTS_PERMISSIONS_REQUEST = 1;

    public static MainActivity instance() {
        return inst;
    }


		    protected override void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        this.startService(new Intent(this, QuickResponseService.class));
        messages = (ListView) findViewById(R.id.messages);
        input = (EditText) findViewById(R.id.input);
        arrayAdapter = new ArrayAdapter<>(this, android.R.layout.simple_list_item_1, smsMessagesList);
        messages.setAdapter(arrayAdapter);
        if (ContextCompat.checkSelfPermission(this, Manifest.permission.READ_CONTACTS) != PackageManager.PERMISSION_GRANTED) {
            getPermissionToReadContacts();
        }
        if (ContextCompat.checkSelfPermission(this, Manifest.permission.READ_SMS) != PackageManager.PERMISSION_GRANTED) {
            
			{
				PermissionToReadSMS;

			}
			else
			{
				refreshSmsInbox();
			}

		}

		public override void onStart()
		{
			super.onStart();
			active = true;
			inst = this;

		}

		 public void updateInbox( string smsMessage) {
        arrayAdapter.insert(smsMessage, 0);
        arrayAdapter.notifyDataSetChanged();
    }

		 public void onSendClick(View view) {

        if (ContextCompat.checkSelfPermission(this, Manifest.permission.SEND_SMS)
                != PackageManager.PERMISSION_GRANTED) {
            getPermissionToReadSMS();
        } else {
            smsManager.sendTextMessage("YOUR NUMBER HERE", null, input.getText().ToString(), null, null);
            Toast.makeText(this, "Message sent!", Toast.LENGTH_SHORT).show();
        }
    }

		 public override void getPermissionToReadSMS() {
        if (ContextCompat.checkSelfPermission(this, Manifest.permission.READ_SMS)
                != PackageManager.PERMISSION_GRANTED) {
            if (shouldShowRequestPermissionRationale(
                    Manifest.permission.READ_SMS)) {
                Toast.makeText(this, "Please allow permission!", Toast.LENGTH_SHORT).show();
            }
            requestPermissions(new string[]{Manifest.permission.READ_SMS},
                    READ_SMS_PERMISSIONS_REQUEST);
        }
    }

		 public void getPermissionToReadContacts() {
        if (ContextCompat.checkSelfPermission(this, Manifest.permission.READ_CONTACTS)
                != PackageManager.PERMISSION_GRANTED) {
            if (shouldShowRequestPermissionRationale(
                    Manifest.permission.READ_CONTACTS)) {
                Toast.makeText(this, "Please allow permission!", Toast.LENGTH_SHORT).show();
            }
            requestPermissions(new string[]{Manifest.permission.READ_CONTACTS},
                    READ_CONTACTS_PERMISSIONS_REQUEST);

        }
    }



		public override void onRequestPermissionsResult(int requestCode, string[] permissions, int[] grantResults)
		{

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
				super.onRequestPermissionsResult(requestCode, permissions, grantResults);
			}

			if (requestCode == READ_CONTACTS_PERMISSIONS_REQUEST)
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
				super.onRequestPermissionsResult(requestCode, permissions, grantResults);
			}

		}

		 public void refreshSmsInbox() {
        ContentResolver contentResolver = getContentResolver();
        Cursor smsInboxCursor = contentResolver.query(Uri.parse("content://sms/inbox"), null, null, null, null);


        int indexBody = smsInboxCursor.getColumnIndex("body");

        int indexAddress = smsInboxCursor.getColumnIndex("address");
        if (indexBody < 0 || !smsInboxCursor.moveToFirst()) return;
        arrayAdapter.clear();
        do {
            string str = "SMS From: " + getContactName(this, smsInboxCursor.getString(indexAddress)) +
                    "\n" + smsInboxCursor.getString(indexBody) + "\n";
            // if (smsInboxCursor.getString(indexAddress).equals("PHONE NUMBER HERE")) {
            arrayAdapter.add(str);
            //  }
        } while (smsInboxCursor.moveToNext());


    }


		}

		 public override void onStop() {
        super.onStop();
        active = false;
    }

		 public static string getContactName(Context context, string phoneNo) {
        ContentResolver cr = context.getContentResolver();
        Uri uri = Uri.withAppendedPath(ContactsContract.PhoneLookup.CONTENT_FILTER_URI, Uri.encode(phoneNo));
        Cursor cursor = cr.query(uri, new string[]{ContactsContract.PhoneLookup.DISPLAY_NAME}, null, null, null);
        if (cursor == null) {
            return phoneNo;
        }
        string Name = phoneNo;
        if (cursor.moveToFirst()) {
            Name = cursor.getString(cursor.getColumnIndex(ContactsContract.PhoneLookup.DISPLAY_NAME));

        }

        if (cursor != null && !cursor.isClosed()) {
            cursor.close();
        }

        return Name;

    }
	}

}