<?xml version="1.0" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="html" indent="yes" omit-xml-declaration="yes" encoding="ISO-8859-1" />
	<xsl:template match="/">
		<STYLE>
			BODY{font:xx-small 'Verdana';}
			.muser{background-color: floralwhite; width:  100%; padding: 3px; border: lightgrey 1px solid;}
			.user{color:black; width: 100%;}
			.data{color:dimgray}
			.command{color: midnightblue; padding-left: 5px; line-height: 170%; width:  100%;}
			.event{color: darkslategray; padding-left: 5px; line-height: 170%; width:  100%;}
			.error{color: red; background-color: white;}
		</STYLE>
		<BODY>
			<xsl:for-each select="user">
				<span class="muser">
					<div class="user">
						<b>
							<xsl:value-of select="@alias"></xsl:value-of>
						</b>
						-
						<b>
							<xsl:value-of select="@institution"></xsl:value-of>
						</b>
					</div>
					<xsl:for-each select="command | error">
						<xsl:choose>
							<xsl:when test="name() = 'command'">
								<div class="command">
									<span class="data">
										<xsl:call-template name="FormatTime">
											<xsl:with-param name="time" select="@time"></xsl:with-param>
										</xsl:call-template>
									</span>
									<xsl:value-of select="@name" />
									<xsl:for-each select="error">
										<div class="error">
											<b>
												<xsl:value-of select="."></xsl:value-of>
											</b>
										</div>
									</xsl:for-each>
								</div>
								<xsl:for-each select="Event">
									<div class="event">
										<span class="data">
											<xsl:call-template name="FormatTime">
												<xsl:with-param name="time" select="@time"></xsl:with-param>
											</xsl:call-template>
										</span>
										&gt;&#160;<xsl:value-of select="@name" />
									</div>
								</xsl:for-each>
							</xsl:when>
							<xsl:otherwise>
								<div class="command">
									<div class="error">
										<b>
											<xsl:value-of select="."></xsl:value-of>
										</b>
									</div>
								</div>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:for-each>
				</span>
				<br />
				<br />
			</xsl:for-each>
		</BODY>
	</xsl:template>
	<xsl:template name="FormatTime">
		<xsl:param name="time"></xsl:param>
		<xsl:variable name="hour" select="substring(@time, 9, 2)" />
		<xsl:variable name="minute" select="substring(@time, 11, 2)" />
		<xsl:variable name="second" select="substring(@time, 13, 2)" />
		<xsl:value-of select="concat($hour, ':', $minute, ':', $second, ' ')" />
	</xsl:template>
	<xsl:template name="FormatDate">
		<xsl:param name="time"></xsl:param>
		<xsl:variable name="year" select="substring(@time, 3, 2)" />
		<xsl:variable name="month" select="substring(@time, 5, 2)" />
		<xsl:variable name="day" select="substring(@time, 7, 2)" />
		<xsl:variable name="hour" select="substring(@time, 9, 2)" />
		<xsl:variable name="minute" select="substring(@time, 11, 2)" />
		<xsl:variable name="second" select="substring(@time, 13, 2)" />
		<xsl:value-of select="concat($day, '/', $month, '/', $year, ' ', $hour, ':', $minute, ':', $second, ' ')" />
	</xsl:template>
</xsl:stylesheet>