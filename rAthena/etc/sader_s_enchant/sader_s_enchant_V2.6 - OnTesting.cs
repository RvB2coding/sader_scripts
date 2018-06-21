//===== rAthena Script =======================================
//= saders enchant npc
//===== By: ==================================================
//= Sader1992
//https://rathena.org/board/profile/30766-sader1992/
//===== Current Version: =====================================
//= 2.6
//===== Compatible With: ===================================== 
//= rAthena Project
//https://rathena.org/board/files/file/3602-saders-enchantment-npc/
//https://github.com/sader1992/sader_scripts
//===== Description: =========================================
//============================================================
//============================================================
prontera,157,176,6	script	sader enchant	998,{
	disable_items;
	if(.s_only_vip){
		if(!vip_status(VIP_STATUS_ACTIVE)){
			mes "this service only for vip";
			close;
		}
	}
	if (BaseLevel < .s_level_required[0]){
		mes "Your level is too Low.";
		mes "   ";
		mes "Minimum level "+.s_level_required[0]+".";
		close;
	}else if(BaseLevel > .s_level_required[1]){
		mes "Your level is too High.";
		mes "   ";
		mes "Maximum level "+.s_level_required[1]+".";
		close;
	}
	mes "Hello!";
	mes "Do you want to enchant you items!";
	mes "I am the best enchanter in the world!";
	next;
	if(.s_zeny > 0)
		mes "this will cost you " + .s_zeny + " Zeny only!";
	if(.item_is_required)
		mes "and an enchantment orb";
	mes "i will do my best to enchant it Successfully!";
	mes "but remember";
	mes "There is luck in this work too.";
	next;
	mes "please if you have items same";
	mes "as the item you want to enchant";
	mes "but them in the storage and come back to me!";
	next;
	.@string$[0] = "Enchant";
	if(.remove_enchant)
		.@string$[1] = "Remove Enchant";
	if(.enable_the_shop)
		.@string$[2] = "The Items you can enchant";
	mes "so what you want to do!";
	menu .@string$[0],L_Enchant,.@string$[1],L_Remove,.@string$[2],-;
	callsub Q_shop; end;
	L_Remove: .@remove_orbs = true;
	L_Enchant: 
	next;
	mes "please select the item you want to enchant";
	for(.@i=0; .@i<getarraysize(.s_all$); .@i++)
		if(getequipid(.s_all_loc[.@i])>-1) {
			set .@armor_menu$, .@armor_menu$ + .s_all$[.@i] + " - [ ^E81B02" + getitemname(getequipid(.s_all_loc[.@i])) + "^000000 ]:";
		}else{
			set .@armor_menu$, .@armor_menu$ + .s_all$[.@i] + " - [ ^D6C4E8" + "No Equip" + "^000000 ]:";
		}
	set .@s_all_selected, select(.@armor_menu$) -1;
	if(getequipid(.s_all_loc[.@s_all_selected])< 0){
		mes "you don't have item equiped there";
		close;
	}
	if (countitem(getequipid(.s_all_loc[.@s_all_selected])) > 1){
			mes "you have more then one item";
			mes "from the item that you want to enchant";
			close;
	}
	.@s_item_refine = getequiprefinerycnt(.s_all_loc[.@s_all_selected]);
	if( getd(".specific_" + .s_all$[.@s_all_selected] + "s") ==1){
		for(.@i=0;.@i<getarraysize(getd("." + .s_all$[.@s_all_selected] + "s$"));.@i++){
			if(getequipid(.s_all_loc[.@s_all_selected]) == atoi(getd("." + .s_all$[.@s_all_selected] + "s$["+.@i+"]"))){
				.@good_to_go = true;
			}
		}
	}else{
		for(.@i=0;.@i<getarraysize(.black_list$);.@i++){
			if(getequipid(.s_all_loc[.@s_all_selected]) == atoi(.black_list$[.@i])){
				.@black_list_item = true;
			}
		}
		.@good_to_go = true;
	}
	if(!.@good_to_go || .@black_list_item){
		mes "sorry";
		mes " i can't enchant this item.";
		close;
	}
	.@card0 = getequipcardid(.s_all_loc[.@s_all_selected],0);
	.@card1 = getequipcardid(.s_all_loc[.@s_all_selected],1);
	.@card2 = getequipcardid(.s_all_loc[.@s_all_selected],2);
	.@card3 = getequipcardid(.s_all_loc[.@s_all_selected],3);
	if(.@remove_orbs){
		next;
		mes "this will remove all the cards and orbs inside the item!";
		if (.s_zeny_remove > 0) {
			mes "this will cost you " + .s_zeny_remove + " Zeny.";
		}
		mes "are you sure?";
			switch(select("NO:Yes")){
				case 1: end;
				case 2:
					mes "for the last time!";
					mes "are you sure?";
					switch(select("NO:Yes")){
					case 1: end;
					case 2: 
						if (Zeny < .s_zeny_remove) {
							mes "Sorry, but you don't have enough zeny.";
							close;
						}
						if(.select_remove_orb){
							if(.@card0 == 0).@card0$ = " - [ ^D6C4E8" + "No Equip" + "^000000 ]:"; else .@card0$ = getitemname(.@card0);
							if(.@card1 == 0).@card1$ = " - [ ^D6C4E8" + "No Equip" + "^000000 ]:"; else .@card1$ = getitemname(.@card1);
							if(.@card2 == 0).@card2$ = " - [ ^D6C4E8" + "No Equip" + "^000000 ]:"; else .@card2$ = getitemname(.@card2);
							if(.@card3 == 0).@card3$ = " - [ ^D6C4E8" + "No Equip" + "^000000 ]:"; else .@card3$ = getitemname(.@card3);
							switch(select(.@card0$,.@card1$,.@card2$,.@card3$)){
								case 1: .@card0 = 0; break;
								case 2: .@card1 = 0; break;
								case 3: .@card2 = 0; break;
								case 4: .@card3 = 0; break;
							}
							specialeffect2 EF_REPAIRWEAPON;
							set .@item, getequipid(.s_all_loc[.@s_all_selected]);
							delitem .@item,1;
							getitem2 .@item, 1, 1, .@s_item_refine, 0, .@card0, .@card1, .@card2, .@card3;
							set Zeny, Zeny-.s_zeny_remove;
							end;
						}
						specialeffect2 EF_REPAIRWEAPON;
						set .@item, getequipid(.s_all_loc[.@s_all_selected]);
						delitem .@item,1;
						getitem2 .@item, 1, 1, .@s_item_refine, 0, 0, 0, 0, 0;
						set Zeny, Zeny-.s_zeny_remove;
						end;
					}
			}
	}
	if(.chosse_orb){
		next;
		mes "select the orb you want";
		for(.@i=0; .@i<getarraysize(getd("." + .s_all$[.@s_all_selected] + "$")); .@i++)
				set .@orb_menu$, .@orb_menu$ + getitemname(atoi(getd("." + .s_all$[.@s_all_selected] + "$["+.@i+"]"))) + ":";
		set .@s_orb_selected, select(.@orb_menu$) -1;
		.@selected_orb_id = getd("." + .s_all$[.@s_all_selected] + "$["+.@s_orb_selected+"]");
	}else{
		.@selected_orb_size = rand(getarraysize(getd("." + .s_all$[.@s_all_selected] + "$")));
		.@selected_orb_id = getd("." + .s_all$[.@s_all_selected] + "$["+.@selected_orb_size+"]");
	}
	next;
	mes "which slot ?";
	for(.@i=getd(".slot_count_" + .s_all$[.@s_all_selected]); .@i<4; .@i++)
		if(getequipcardid(.s_all_loc[.@s_all_selected],.@i)!= null) {
			set .@slot_menu$, .@slot_menu$ + " [ ^E81B02" + getitemname(getequipcardid(.s_all_loc[.@s_all_selected],.@i)) + "^000000 ]:";
		}else{
			set .@slot_menu$, .@slot_menu$ + " [ ^D6C4E8" + "Empty" + "^000000 ]:";
		}
	set .@s_slot_selected, select(.@slot_menu$) -1;
	.@s_slot_selected += getd(".slot_count_" + .s_all$[.@s_all_selected]);
	if(!.s_enchant_overwrite){
		if(getequipcardid(.s_all_loc[.@s_all_selected],.@s_slot_selected) > 0){
			mes "you already have orb in this slot";
			close;
		}
	}
	if (Zeny < .s_zeny) {
			mes "Sorry, but you don't have enough zeny.";
			close;
		}
	if(.item_is_required && .chosse_orb){
		if (countitem(.@selected_orb_id) < 1){
			mes"you don't have enchant orb";
			close;
		}
	}
	close2;
	specialeffect2 EF_MAPPILLAR;
	progressbar "ffff00",.progress_time;
	set Zeny, Zeny-.s_zeny;
	if(.item_is_required && .chosse_orb){delitem .@selected_orb_id,1;}
	if (rand(100) < .success_chanse[.@s_slot_selected]){
		mes "We did it!";
		specialeffect2 154;
		setd(".@card" + .@s_slot_selected, .@selected_orb_id);
		set .@item, getequipid(.s_all_loc[.@s_all_selected]);
		delitem .@item,1;
		getitem2 .@item, 1, 1, .@s_item_refine, 0, .@card0, .@card1, .@card2, .@card3;
		equip .@item;
		close;
	}else{
		specialeffect2 155;
		mes "I am sorry";
		mes "We did Fail";
		specialeffect2 EF_PHARMACY_FAIL;
		if (rand(100) < .brack_chance){
			set .@item, getequipid(.s_all_loc[.@s_all_selected]);
			delitem .@item,1;
			mes "and it broke!!";
			specialeffect EF_SUI_EXPLOSION;
		}
		close;
	}
	
	
Q_shop:
	switch(select("Weapons:Armors:Shields:Germents:Shoses:Accessarys:Uppers:Middels:Lowers")){
	case 1: callshop "enchantable_items_Weapon",1; break;
	case 2: callshop "enchantable_items_Armor",1; break;
	case 3: callshop "enchantable_items_Shield",1; break;
	case 4: callshop "enchantable_items_Germent",1; break;
	case 5: callshop "enchantable_items_Shose",1; break;
	case 6: callshop "enchantable_items_Accessary",1; break;
	case 7: callshop "enchantable_items_Upper",1; break;
	case 8: callshop "enchantable_items_Middel",1; break;
	case 9: callshop "enchantable_items_Lower",1; break;
	}
end;	

OnInit:
	//--------------------------------------------------------------//
	//--------------------------------------------------------------//
	//--------------------   configuration   -----------------------//
	//--------------------------------------------------------------//
	//--------------------------------------------------------------//
	
	//--------------------------------------------------------------//
	//if you want to remove one from the menu you need to remove it down too!! /or add
	//--------------------------------------------------------------//
	setarray .s_all$,"Weapon","Armor","Shield","Germent","Shose","Accessary","Upper","Middel","Lower";
	setarray .s_all_loc,EQI_HAND_R,EQI_ARMOR,EQI_HAND_L,EQI_GARMENT,EQI_SHOES,EQI_ACC_L,EQI_HEAD_TOP,EQI_HEAD_MID,EQI_HEAD_LOW;
	
	//--------------------------------------------------------------//
	//Orbs IDs (Note : Shield = left hand so the weapon on the left hand count as Shield too!
	//--------------------------------------------------------------//
	setarray .Weapon$,4741,4933,4861,4762,4934;	//right handed weapons
	setarray .Armor$,4933,4861,4762,4934;	//Armors
	setarray .Shield$,4861,4762,4934;	//Shields and left hand weapons
	setarray .Germent$,4741,4933,4861,4762,4934;	//Germent
	setarray .Shose$,4741,4933,4861,4762,4934;	//Shose
	setarray .Accessary$,4741,4933,4861,4762,4934;	//orbs id
	setarray .Upper$,4741,4933,4861,4762,4934;	//Accessary
	setarray .Middel$,4741,4933,4861,4762,4934;	//Middel
	setarray .Lower$,4741,4933,4861,4762,4934;	//Lower
	
	//--------------------------------------------------------------//
	//if you want to put specific IDs for kind of gear put it to 1
	//--------------------------------------------------------------//
	.specific_Weapons = true;
	.specific_Armors = true;
	.specific_Shields = true;
	.specific_Germents = true;
	.specific_Shoses = true;
	.specific_Accessarys = true;
	.specific_Uppers = true;
	.specific_Middels = true;
	.specific_Lowers = true;
	
	//--------------------------------------------------------------//
	//if specific put the IDs here
	//--------------------------------------------------------------//
	setarray .Weapons$,1601,1201,1204,1207,1210,1213,1216,1219,1222,1247,1248,1249;	//right handed weapons
	setarray .Armors$,2301,2303,2305,2307,2307,2309,2312,2314,2316,2321,2323,2325,2328,2330,2332;	//Armors
	setarray .Shields$,2101,2103,2105,2107,2113,2117;	//Shields and left hand weapons
	setarray .Germents$,2512,2501,2503,2505;	//Germents
	setarray .Shoses$,2416,2401,2403,2405,2411;	//Shoses
	setarray .Accessarys$,2628,2608,2609,2612,2613,2627;	//Accessarys
	setarray .Uppers$,2206,2208,2211,2216;	//Uppers
	setarray .Middels$,2218,2241;	//Middels
	setarray .Lowers$,2628,2206;	//Lowers
	
	//--------------------------------------------------------------//
	//if not specific put the black list IDs here (if you want
	//--------------------------------------------------------------//
	setarray .black_list$,2335,2338,2340,2341;
	
	//--------------------------------------------------------------//
	//here you can make a specific slot number for each kind
	//0 = all 4 slot ,1 = last 3 slot ,2 = last 2 slot ,3 = last 1 slot
	//--------------------------------------------------------------//
	.slot_count_Weapon = 0;
	.slot_count_Armor = 0;
	.slot_count_Shield = 0;
	.slot_count_Germent = 0;
	.slot_count_Shose = 0;
	.slot_count_Accessary = 0;
	.slot_count_Upper = 0;
	.slot_count_Middel = 0;
	.slot_count_Lower = 0;
	
	//--------------------------------------------------------------//
	//other configuration
	//--------------------------------------------------------------//
	setarray .s_level_required,0,175;	//the level required to use the npc
	.s_only_vip = false;	//if you want only vip to use it put it to 1
	setarray .success_chanse,100,80,60,40;	//success chanse [1st_slot_chanse,2nd_slot_chanse,3rd_slot_chanse,4th_slot_chanse]
	.s_zeny = 10000;	//if you don't want zeny requirment set it to 0
	.s_zeny_remove = 100000;	//this for enchantment reset
	.item_is_required = false;	//if you want the orb it self to be required true = yes , false = no(if .chosse_orb = false this will be false too)
	.s_enchant_overwrite = false;	//if true then you can overwrite the enchant
	.progress_time = 7;	//the time that needed to wait until the socket end
	.chosse_orb = false;	//false = random ,true = yes
	.brack_chance = 50;	//the chanse that it will brack if it fail
	.remove_enchant = true;	//false = no ,true = yes
	.select_remove_orb = true;
	//--------------------------------------------------------------//
	//this will only show the items that the npc can enchant in a shop but no one can buy from it as long as you don't give them the value
	//--------------------------------------------------------------//
	.enable_the_shop = true;


	
	
	
	//--------------------------------------------------------------//
	//Do not edit here
	//--------------------------------------------------------------//
	npcshopdelitem "enchantable_items_Weapon",512;
	npcshopdelitem "enchantable_items_Armor",512;
	npcshopdelitem "enchantable_items_Shield",512;
	npcshopdelitem "enchantable_items_Germent",512;
	npcshopdelitem "enchantable_items_Shose",512;
	npcshopdelitem "enchantable_items_Accessary",512;
	npcshopdelitem "enchantable_items_Upper",512;
	npcshopdelitem "enchantable_items_Middel",512;
	npcshopdelitem "enchantable_items_Lower",512;
	
	for (.@i = 0; .@i < getarraysize(.Weapons$); .@i++)
			npcshopadditem "enchantable_items_Weapon", atoi(.Weapons$[.@i]),1;
	for (.@i = 0; .@i < getarraysize(.Armors$); .@i++)
			npcshopadditem "enchantable_items_Armor", atoi(.Armors$[.@i]),1;
	for (.@i = 0; .@i < getarraysize(.Shields$); .@i++)
			npcshopadditem "enchantable_items_Shield", atoi(.Shields$[.@i]),1;
	for (.@i = 0; .@i < getarraysize(.Germents$); .@i++)
			npcshopadditem "enchantable_items_Germent", atoi(.Germents$[.@i]),1;	
	for (.@i = 0; .@i < getarraysize(.Shoses$); .@i++)
			npcshopadditem "enchantable_items_Shose", atoi(.Shoses$[.@i]),1;	
	for (.@i = 0; .@i < getarraysize(.Accessarys$); .@i++)
			npcshopadditem "enchantable_items_Accessary", atoi(.Accessarys$[.@i]),1;	
	for (.@i = 0; .@i < getarraysize(.Uppers$); .@i++)
			npcshopadditem "enchantable_items_Upper", atoi(.Uppers$[.@i]),1;		
	for (.@i = 0; .@i < getarraysize(.Middels$); .@i++)
			npcshopadditem "enchantable_items_Middel", atoi(.Middels$[.@i]),1;	
	for (.@i = 0; .@i < getarraysize(.Lowers$); .@i++)
			npcshopadditem "enchantable_items_Lower", atoi(.Lowers$[.@i]),1;	
	end;
}
-	pointshop	enchantable_items_Weapon	-1,#YOU_CAN_ENCHANT_Weapons,512:1;
-	pointshop	enchantable_items_Armor	-1,#YOU_CAN_ENCHANT_Armors,512:1;
-	pointshop	enchantable_items_Shield	-1,#YOU_CAN_ENCHANT_Shields,512:1;
-	pointshop	enchantable_items_Germent	-1,#YOU_CAN_ENCHANT_Germents,512:1;
-	pointshop	enchantable_items_Shose	-1,#YOU_CAN_ENCHANT_Shoses,512:1;
-	pointshop	enchantable_items_Accessary	-1,#YOU_CAN_ENCHANT_Accessarys,512:1;
-	pointshop	enchantable_items_Upper	-1,#YOU_CAN_ENCHANT_Uppers,512:1;
-	pointshop	enchantable_items_Middel	-1,#YOU_CAN_ENCHANT_Middels,512:1;
-	pointshop	enchantable_items_Lower	-1,#YOU_CAN_ENCHANT_Lowers,512:1;



