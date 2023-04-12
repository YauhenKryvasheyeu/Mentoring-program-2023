namespace AsyncAwait.Task2.CodeReviewChallenge.Services;

public class PrivacyDataService : IPrivacyDataService
{
    private const string PrivacyData = "This Policy describes how async/await processes your personal data," +
        "but it may not address all possible data processing scenarios.";

    public string GetPrivacyData() => PrivacyData;
}
