using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

[ApiController]
[Route("[controller]")]
public class LocalizationController : ControllerBase
{
    private static readonly Dictionary<string, Dictionary<string, string>> _localizationTexts =
        new Dictionary<string, Dictionary<string, string>>()
    {
        {
            "en", new Dictionary<string, string>
            {
                {   "getPointsButton", "All" },
                {   "getPointsButtonPlywalnia", "Pool" },
                {   "getPointsButtonSilownia", "Gym" },
                {   "getPointsButtonBoisko", "Field" },
                {   "getPointsButtonBudynekSportowy","Sports Building" },
                {   "openSearchB","Open Search" },
                {   "themebutton","Change Theme" },
                {   "readMessagesButton","Notifications" },
                {   "refreshButton","Refresh" },
                {   "trasyButton","Running Trails" },
                {   "activitySelect", "Choose Activity" },
                {   "Kosz","Basketball" },
                {   "Noga","Soccer" },
                {   "Bieg", "Running" },
                {   "Inne", "Other" },
                {   "czaslabel","Time" },
                {   "confirmBtn","Confirm" },
                {   "closeModalBtn","Cancel" },
                {   "ankietabieg","Running Preparation Survey" },
                {   "explevel" ,"Experience Level"},
                {   "energylevel","Motivation Level"},
                {   "sportwearl","Sportswear"},
                {   "closeM" ,"Close"},
                {   "sendM" ,"Send"}
            }
        },
        {
            "pl", new Dictionary<string, string>
            {
                {   "getPointsButton", "Wszystkie" },
                {   "getPointsButtonPlywalnia", "Plywalnia" },
                {   "getPointsButtonSilownia", "Silownia" },
                {   "getPointsButtonBoisko", "Boisko" },
                {   "getPointsButtonBudynekSportowy","Budynki Sportowe" },
                {   "openSearchB","Open Search" },
                {   "themebutton","Change Theme" },
                {   "readMessagesButton","Powiadomienia" },
                {   "refreshButton","Odswiez" },
                {   "trasyButton","Trasy Biegowe" },
                {   "activitySelect", "Wybierz Aktywnosc:" },
                {   "Kosz","Koszykowka" },
                {   "Noga","Pilka Nozna" },
                {   "Bieg", "Bieganie" },
                {   "Inne", "Inne" },
                {   "czaslabel","Time" },
                {   "confirmBtn","Zatwierdz" },
                {   "closeModalBtn","Anuluj" },
                {   "ankietabieg","Ankieta Przygotowania do Biegania" },
                {   "explevel" ,"Poziom doświadczenia"},
                {   "energylevel","Poziom chęci"},
                {   "sportwearl","Ubiór sportowy"},
                {   "closeM" ,"Zamknij"},
                {   "sendM" ,"Wyślij"}
            }
        }
    };

    [HttpGet("page-texts")]
    public IActionResult GetPageTexts([FromQuery] string lang = "en")
    {
        if (!_localizationTexts.ContainsKey(lang))
        {
            lang = "en";
        }

        var texts = _localizationTexts[lang];
        return Ok(texts);
    }
}