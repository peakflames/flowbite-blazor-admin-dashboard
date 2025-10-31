using Microsoft.AspNetCore.Components;
using WebApp.Domain;

namespace WebApp.Services;

/// <summary>
/// Provides strongly typed content for the pricing page.
/// </summary>
public sealed class PricingService
{
    /// <summary>
    /// Returns all data required to render the pricing experience.
    /// </summary>
    public PricingPageData GetPricingPageData()
    {
        var plans = new List<PricingPlan>
        {
            new(
                Title: "Starter",
                Subtitle: "Best option for personal use and for your next project.",
                MonthlyPrice: "$24",
                YearlyPrice: "$200",
                Features: new List<PricingFeature>
                {
                    CreateFeature("Individual configuration", true),
                    CreateFeature("No setup, or hidden fees", true),
                    CreateFeature("""Team size: <span class="font-semibold">1 developer</span>""", true),
                    CreateFeature("""<span class="font-semibold">Premium support</span>""", false),
                    CreateFeature("""<span class="font-semibold">Free updates</span>""", false)
                }),
            new(
                Title: "Company",
                Subtitle: "Relevant for multiple users, extended & premium support.",
                MonthlyPrice: "$49",
                YearlyPrice: "$400",
                Features: new List<PricingFeature>
                {
                    CreateFeature("Individual configuration", true),
                    CreateFeature("No setup, or hidden fees", true),
                    CreateFeature("""Team size: <span class="font-semibold">10 developers</span>""", true),
                    CreateFeature("""Premium support: <span class="font-semibold">24 months</span>""", true),
                    CreateFeature("""Free updates: <span class="font-semibold">24 months</span>""", true)
                }),
            new(
                Title: "Enterprise",
                Subtitle: "For large scale uses and extended redistribution rights.",
                MonthlyPrice: "$499",
                YearlyPrice: "$1500",
                Features: new List<PricingFeature>
                {
                    CreateFeature("Individual configuration", true),
                    CreateFeature("No setup, or hidden fees", true),
                    CreateFeature("""Team size: <span class="font-semibold">100 developers</span>""", true),
                    CreateFeature("""Premium support: <span class="font-semibold">36 months</span>""", true),
                    CreateFeature("""Free updates: <span class="font-semibold">36 months</span>""", true)
                })
        };

        var comparisonRows = new List<PricingComparisonRow>
        {
            CreateRow("Seperate business/personal", true, true, true),
            CreateRow("Estimate tax payments", true, true, true),
            CreateRow("Stock control", true, true, true),
            CreateRow("Create invoices & estimates", false, true, true),
            CreateRow("Manage bills & payments", false, true, true),
            CreateRow("Run payroll", false, true, true),
            CreateRow("Handle multiple currencies", false, false, true),
            CreateRow("Number of Users", "1 User", "5-10 Users", "20+ Users"),
            CreateRow("Track deductible mileage", false, false, true),
            CreateRow("Track employee time", false, false, true),
            CreateRow("Multi-device", false, false, true)
        };

        var faqs = new List<FaqItem>
        {
            new("What do you mean by \"Figma assets\"?",
                CreateMarkup("""
                    <p class="text-gray-600 dark:text-gray-300">
                        You will have access to download the full Figma project including all of the pages, the components, responsive pages, and also the icons, illustrations, and images included in the screens.
                    </p>
                """)),
            new("What does \"lifetime access\" exactly mean?",
                CreateMarkup("""
                    <p class="text-gray-600 dark:text-gray-300">
                        Once you have purchased either the design, code, or both packages, you will have access to all of the future updates based on the roadmap, free of charge.
                    </p>
                """)),
            new("How does support work?",
                CreateMarkup("""
                    <p class="text-gray-600 dark:text-gray-300">
                        We're aware of the importance of well qualified support, that is why we decided that support will only be provided by the authors that actually worked on this project.
                    </p>
                    <p class="text-gray-600 dark:text-gray-300">
                        Feel free to <a href="/" class="text-primary-600 dark:text-primary-500 font-medium underline hover:no-underline" target="_blank" rel="noreferrer">contact us</a> and we'll help you out as soon as we can.
                    </p>
                """)),
            new("I want to build more than one project with Flowbite. Is that allowed?",
                CreateMarkup("""
                    <p class="text-gray-600 dark:text-gray-300">
                        You can use Flowbite for an unlimited amount of projects, whether it's a personal website, a SaaS app, or a website for a client. As long as you don't build a product that will directly compete
                        with Flowbite either as a UI kit, theme, or template, it's fine.
                    </p>
                    <p class="text-gray-600 dark:text-gray-300">
                        Find out more information by <a href="/" class="text-primary-600 dark:text-primary-500 font-medium underline hover:no-underline">reading the license</a>.
                    </p>
                """)),
            new("What does \"free updates\" include?",
                CreateMarkup("""
                    <p class="text-gray-600 dark:text-gray-300">
                        The free updates that will be provided is based on the <a href="/" class="text-primary-600 dark:text-primary-500 font-medium underline hover:no-underline">roadmap</a> that we have laid out for this project.
                        It is also possible that we will provide extra updates outside of the roadmap as well.
                    </p>
                """)),
            new("What does the free version include?",
                CreateMarkup("""
                    <p class="text-gray-600 dark:text-gray-300">
                        The <a href="/" class="text-primary-600 dark:text-primary-500 font-medium underline hover:no-underline">free version</a> of FlowBite includes a minimal style guidelines, component variants, and a
                        dashboard page with the mobile version alongside it.
                    </p>
                    <p class="text-gray-600 dark:text-gray-300">
                        You can use this version for any purposes, because it is open-source under the MIT license.
                    </p>
                """)),
            new("What is the difference between FlowBite and Tailwind UI?",
                CreateMarkup("""
                    <p class="text-gray-600 dark:text-gray-300">
                        Although both FlowBite and Tailwind UI are built for integration with Tailwind CSS, the main difference is in the design, the pages, the extra components and UI elements that FlowBite includes.
                    </p>
                    <p class="text-gray-600 dark:text-gray-300">
                        Additionally, FlowBite is a project that is still in development, and later it will include both the application, marketing, and e-commerce UI interfaces.
                    </p>
                """)),
            new("How do I purchase a license for my entire team?",
                CreateMarkup("""
                    <p class="text-gray-600 dark:text-gray-300">You can purchase a license that you can share with your entire team here:</p>
                    <ul class="list-disc pl-4">
                        <li class="mb-2 text-gray-600">
                            <span class="text-primary-600 dark:text-primary-500 cursor-pointer font-medium hover:underline">Figma Files - Buy a team license for $299 USD</span>
                        </li>
                        <li class="mb-2 text-gray-600">
                            <span class="text-primary-600 dark:text-primary-500 cursor-pointer font-medium hover:underline">Figma Files + Tailwind CSS code pre-order - Buy a team license for <del>$699</del> $559 USD</span>
                        </li>
                        <li class="text-gray-600 dark:text-gray-300">
                            <span class="text-primary-600 dark:text-primary-500 cursor-pointer font-medium hover:underline">Tailwind CSS code pre-order - Buy a team license for <del>$399</del> $319 USD</span>
                        </li>
                    </ul>
                    <p class="text-gray-600 dark:text-gray-300">Please use a single account to share with your team to access the download files.</p>
                """)),
            new("Can I build/sell templates or themes using FlowBite?",
                CreateMarkup("""
                    <p class="text-gray-600 dark:text-gray-300">It is not allowed to use FlowBite or parts of the project to build themes, templates, UI kits, or page builders.</p>
                    <p class="text-gray-600 dark:text-gray-300">
                        Find out more information by <a href="/" class="text-primary-600 dark:text-primary-500 font-medium underline hover:no-underline">reading the license</a>.
                    </p>
                """)),
            new("Can I use FlowBite in open-source projects?",
                CreateMarkup("""
                    <p class="text-gray-600 dark:text-gray-300">
                        Generally, it is accepted to use FlowBite for open-source projects, but please make sure to credit FlowBite in the README and contact us for any clarifications.
                    </p>
                """)),
            new("Is there a free trial available?",
                CreateMarkup("""
                    <p class="text-gray-600 dark:text-gray-300">
                        Yes, you can request a free trial by contacting our support team. We will evaluate your use case and grant temporary access where applicable.
                    </p>
                """)),
            new("What if I am not satisfied with FlowBite?",
                CreateMarkup("""
                    <p class="text-gray-600 dark:text-gray-300">
                        We offer a 30-day money-back guarantee. If FlowBite does not meet your expectations, reach out within
                        30 days and we'll issue a full refund.
                    </p>
                """)),
            new("Is it allowed to use the design assets, such as the fonts, icons, and illustrations?",
                CreateMarkup("""
                    <p class="text-gray-600 dark:text-gray-300">
                        What you see is what you get. All icons, fonts, and illustrations can be used based on the licensing that we researched or purchased. For example, we purchased rights to use the illustrations in Flowbite.
                    </p>
                """)),
            new("Where can I access my download files?",
                CreateMarkup("""
                    <p class="text-gray-600 dark:text-gray-300">After you purchased one of the plans, you will get two emails: one for the invoice, and another one with the download files.</p>
                    <p class="text-gray-600 dark:text-gray-300">Soon we will create a way that you will be able to access the download files from the FlowBite dashboard from this website.</p>
                """)),
            new("I have a company registered for VAT. Where can I add the VAT for the invoice?",
                CreateMarkup("""
                    <p class="text-gray-600 dark:text-gray-300">
                        After initializing the checkout process from Paddle, you will be able to see a text "Add VAT code". Click on that, and add the VAT code for your company. This will also remove the extra VAT tax from the purchase.
                    </p>
                """)),
            new("Why would I pre-order the Tailwind CSS code?",
                CreateMarkup("""
                    <p class="text-gray-600 dark:text-gray-300">
                        If you decide to pre-order the Tailwind CSS code, you can get a base 20% price reduction and purchase it only for $119, instead of $149.
                    </p>
                """))
        };

        var footerMenus = new List<FooterMenu>
        {
            new("Resources", new List<FooterLink>
            {
                new("Flowbite", "/", "mb-4"),
                new("Figma", "/", "mb-4"),
                new("Tailwind CSS", "/", "mb-4"),
                new("Blog", "/", "mb-4"),
                new("Blocks", "/", "mb-4")
            }),
            new("Help and support", new List<FooterLink>
            {
                new("GitHub Repository", "/", "mb-4"),
                new("Flowbite Library", "/", "mb-4")
            }),
            new("Follow us", new List<FooterLink>
            {
                new("GitHub", "/", "mb-4"),
                new("Twitter", "/", "mb-4"),
                new("Facebook", "/", "mb-4"),
                new("LinkedIn", "/", "mb-4")
            }),
            new("Legal", new List<FooterLink>
            {
                new("Privacy Policy", "/", "mb-4"),
                new("Terms & Conditions", "/", "mb-4"),
                new("EULA", "/", "mb-4")
            })
        };

        var footerDescription = CreateMarkup("""
            <p>
                Flowbite is a UI library of elements & components based on Tailwind CSS that can get you started building websites faster and more efficiently.
            </p>
        """);

        return new PricingPageData(
            Path: "/pages/pricing",
            Title: "Flowbite Svelte Admin Dashboard - Pricing",
            Subtitle: "Pricing",
            Description: "Pricing example - Flowbite Svelte Admin Dashboard",
            HeroTitle: "Our pricing plan made simple",
            HeroLeadingText: "All types of businesses need access to development resources, so we give you the option to decide how much you need to use.",
            Plans: plans,
            ComparisonRows: comparisonRows,
            Faqs: faqs,
            FooterMenus: footerMenus,
            Brand: new FooterBrand(
                Name: "Flowbite",
                Href: "https://flowbite.com",
                Src: "/favicon.png",
                Alt: "Flowbite Logo"),
            FooterDescription: footerDescription);
    }

    private static PricingFeature CreateFeature(string content, bool isAvailable) =>
        new(new MarkupString(content), isAvailable);

    private static PricingComparisonRow CreateRow(string name, bool freelancer, bool company, bool enterprise) =>
        new(
            name,
            CreateBooleanValue(freelancer),
            CreateBooleanValue(company),
            CreateBooleanValue(enterprise));

    private static PricingComparisonRow CreateRow(string name, string freelancer, string company, string enterprise) =>
        new(
            name,
            new PricingComparisonValue(null, freelancer),
            new PricingComparisonValue(null, company),
            new PricingComparisonValue(null, enterprise));

    private static PricingComparisonValue CreateBooleanValue(bool value) =>
        new(value, null);

    private static MarkupString CreateMarkup(string html) =>
        new(html);
}
